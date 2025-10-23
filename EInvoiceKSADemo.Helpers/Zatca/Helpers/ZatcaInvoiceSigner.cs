/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaInvoiceSigner : IZatcaInvoiceSigner
    {
        private readonly IInvoiceSigner _signer;
        public ZatcaInvoiceSigner(IInvoiceSigner signer)
        {
            this._signer = signer;
        }
        public ZatcaInvoiceResult SignInvoice(string xmlContent, string Facility_ID, int Environment, string Branch_ID)
        {
            var result = _signer.Sign(xmlContent, Facility_ID, Environment, Branch_ID);

            if (result.IsValid)
            {
                var invoiceHash = result.Steps.Find(s => s.IsValid && s.StepType == Models.StepType.InvoiceHash).ResultValue;
                var qrCodeContent = result.Steps.Find(s => s.IsValid && s.StepType == Models.StepType.QrCode).ResultValue;
                var invoiceAsBase64 = result.Steps.Find(s => s.IsValid && s.StepType == Models.StepType.InvoiceBase64).ResultValue;
                return new ZatcaInvoiceResult
                {
                    Success = true,
                    Data = new ZatcaInvoiceModel
                    {
                        SignedXml = result.SingedXML,
                        InvoiceHash = invoiceHash,
                        QrCode = qrCodeContent
                    }
                };
            }
            var stepErrorMessages = string.Join(",", result.Steps.Select(s => s.ErrorMessage));
            return new ZatcaInvoiceResult
            {
                Success = false,
                Message = string.Join(",", result.ErrorMessage, stepErrorMessages)
            };
        }

        public ZatcaInvoiceResult SignInvoice(string xmlContent, string certificateContent, string privateKeyContent)
        {
            throw new NotImplementedException();
        }
    }
}
