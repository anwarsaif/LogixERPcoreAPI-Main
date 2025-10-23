/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Models;
using EInvoiceKSADemo.Helpers.Zatca.Constants;
using EInvoiceKSADemo.Helpers.Zatca.Models;
using EInvoiceKSADemo.Helpers.Zatca.Services;
using EInvoiceKSADemo.NETFramework.Helpers.Zatca.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaReporter : IZatcaReporter
    {
        private readonly IZatcaAPICaller _apiCaller;
        private readonly IXmlInvoiceGenerator _xmlGenerator;
        private readonly IInvoiceSigner _signer;

        public ZatcaReporter(IZatcaAPICaller apiCaller,
            IXmlInvoiceGenerator xmlGenerator, IInvoiceSigner signer)
        {
            this._apiCaller = apiCaller;
            this._xmlGenerator = xmlGenerator;
            this._signer = signer;
        }
        //public ZatcaReporter()
        //{
        //    this._apiCaller = new ZatcaAPICaller();
        //    this._xmlGenerator = new XmlInvoiceGenerator();
        //    this._signer = new InvoiceSigner();
        //}

        //public ZatcaReporter(ICertificateConfiguration config)
        //{
        //    this._apiCaller = new ZatcaAPICaller();
        //    this._xmlGenerator = new XmlInvoiceGenerator();
        //    this._signer = new InvoiceSigner(config);
        //}

        public async Task<ZatcaInvoiceReportResult> ReportInvoiceAsync<T>(T model, string Facility_ID, int Environment, string Branch_ID) where T : class
        {
            try
            {
                ZatcaLogger.Log("Start Generating Xml");
                string invoiceXml = null;
                try
                {
                    var xmlStream = ZatcaUtility.ReadInternalEmbededResourceStream(XslSettings.Embeded_InvoiceXmlFile);
                    invoiceXml = _xmlGenerator.GenerateInvoiceAsXml(xmlStream, model);
                    ZatcaLogger.Log(invoiceXml);
                }
                catch (Exception ex)
                {
                    ZatcaLogger.Log(ex);
                    return new ZatcaInvoiceReportResult { Message = "Xml Error : " + ex.Message };
                }
                ZatcaLogger.Log("End Generating Xml");

                // 02- Sign XML
                ZatcaLogger.Log("Start Signing");
                var signingResult = _signer.Sign(invoiceXml, Facility_ID, Environment, Branch_ID);
                ZatcaLogger.Log("End Signing");

                // 03- Report to API
                if (signingResult.IsValid)
                {
                    ZatcaLogger.Log("Start Sending to Zatca");
                    int clearanceStatus = GetClearanceStatus(signingResult);
                    var apiResult = await _apiCaller.ReportSingleInvoiceAsync(new InputInvoiceModel
                    {
                        Invoice = signingResult.InvoiceAsBase64,
                        InvoiceHash = signingResult.InvoiceHash,
                        UUID = signingResult.UUID,
                    }, clearanceStatus);
                    ZatcaLogger.Log("Start Sending to Zatca");
                    if (apiResult != null)
                    {
                        //Save File 
                      //  SaveXmlFile(signingResult);

                        // 04- return results
                        return new ZatcaInvoiceReportResult
                        {
                            Success = signingResult.IsSimplified ? apiResult.ReportingStatus == "REPORTED" : apiResult.ClearanceStatus == "CLEARED",
                            Data = new ZatcaInvoiceModel
                            {
                                InvoiceHash = signingResult.InvoiceHash,
                                QrCode = signingResult.IsSimplified ? signingResult.QrCode : getQrCode(apiResult.ClearedInvoice),
                                SignedXml = signingResult.SingedXML,
                                ReportingStatus = signingResult.IsSimplified ? apiResult.ReportingStatus : apiResult.ClearanceStatus,
                                ReportingResult = JsonSerializer.Serialize(apiResult),
                                SubmissionDate = DateTime.Now,
                                IsReportedToZatca = signingResult.IsSimplified ? apiResult.ReportingStatus == "REPORTED" : apiResult.ClearanceStatus == "CLEARED",
                                ErrorMessages = apiResult.ValidationResults.ErrorMessages,
                                WarningMessages = apiResult.ValidationResults.WarningMessages
                            }
                        };
                    }
                }
                return new ZatcaInvoiceReportResult() { Success = false, Message = signingResult.ErrorMessage };
            }
            catch (Exception ex)
            {
                return new ZatcaInvoiceReportResult() { Success = false, Message = ex.Message };
            }
        }

        private string getQrCode(string clearedInvoice)
        {
            if (!string.IsNullOrEmpty(clearedInvoice))
            {
                var signedXml = Encoding.UTF8.GetString(Convert.FromBase64String(clearedInvoice));
                if (!string.IsNullOrEmpty(signedXml))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(signedXml);

                    var qrCode = ZatcaUtility.GetNodeInnerText(xmlDoc, XslSettings.QR_CODE_XPATH);
                    return qrCode;
                }
            }
            return null;
        }

        private static void SaveXmlFile(ZatcaResult signingResult)
        {
            var fileName = signingResult.IsSimplified ? "Simplified" : "Standard";
            if (!Directory.Exists(@"C:\Invoice Files3\"))
            {
                Directory.CreateDirectory(@"C:\Invoice Files3\");
            }
            var pathToSave = $@"C:\Invoice Files3\{fileName + "_"+ Guid.NewGuid().ToString()}.xml";
            File.WriteAllText(pathToSave, signingResult.SingedXML);
        }

        private int GetClearanceStatus(ZatcaResult signingResult)
        {
            return signingResult.IsSimplified ? 0 : 1;
        }
    }
}
