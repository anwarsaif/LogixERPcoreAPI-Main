/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using EInvoiceKSADemo.Helpers.Zatca.Models;
using EInvoiceKSADemo.NETFramework.Helpers.Zatca.Logging;

namespace EInvoiceKSADemo.Helpers.Zatca
{
    public class ZatcaHttpClient
    {
        public static string LastErrorMessage { get; private set; }

        public static async Task<TResult> PostAsync<TResult, TInput>(string url, TInput model, IDictionary<string, string> headers, bool requireAuth = false, bool patchHttpMethod = false) where TResult : class
        {
            LastErrorMessage = null;
            HttpResponseMessage response = null;
            try
            {
                ZatcaLogger.Log("Zatca Http Client Started");
                if (string.IsNullOrEmpty(SharedData.APIUrl))
                {
                    LastErrorMessage = "API URL is Null";
                    return null;
                }
                if (requireAuth && (string.IsNullOrEmpty(SharedData.UserName) || string.IsNullOrEmpty(SharedData.Secret)))
                {
                    LastErrorMessage = "User Name Or Secret is Null";
                    return null;
                }
                ZatcaLogger.Log(LastErrorMessage);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SharedData.APIUrl);
                    client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en");

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    if (requireAuth)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Basic",
                            parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{SharedData.UserName}:{SharedData.Secret}")));
                    }

                    if (patchHttpMethod)
                    {
                        var req = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                        req.Content = JsonContent.Create(model);
                        response = await client.SendAsync(req);
                    }
                    else
                    {
                        response = await client.PostAsJsonAsync(url, model);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                        || response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        || (model is InputComplianceModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        || (model is InputCSIDModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest))
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        ZatcaLogger.Log(errorResponse);
                        LastErrorMessage = $"Response: {(int)response.StatusCode}-{response.ReasonPhrase} , Message : {JsonSerializer.Serialize(errorResponse)}";
                        return null;
                    }

                    var result = await response.Content.ReadFromJsonAsync<TResult>();

                    var reportingResult = result as InvoiceModelResult;
                    if (reportingResult != null)
                    {
                        if (reportingResult.ValidationResults.ErrorMessages.Count > 0 || reportingResult.ValidationResults.WarningMessages.Count > 0)
                        {
                            LastErrorMessage = JsonSerializer.Serialize(reportingResult, new JsonSerializerOptions
                            {
                                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic),
                            });
                            // LastErrorMessage = JsonConvert.SerializeObject(reportingResult);
                        }
                    }

                    ZatcaLogger.Log("Zatca Http Client End Successfully!.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                ZatcaLogger.Log(ex);
                LastErrorMessage = $"Response Code :  {(int?)response?.StatusCode} : {response?.ReasonPhrase} , Exception Message : {ex.Message}{ex.InnerException?.Message}";
                ZatcaLogger.Log(LastErrorMessage);
                return null;
            }
        }
    }
}

/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using EInvoiceKSADemo.Helpers.Zatca.Models;
//using EInvoiceKSADemo.NETFramework.Helpers.Zatca.Logging;
//using Newtonsoft.Json;
//using Org.BouncyCastle.Asn1.Cmp;
//using Org.BouncyCastle.Asn1.Crmf;
//using RestSharp;
//namespace EInvoiceKSADemo.Helpers.Zatca
//{
//    public class ZatcaHttpClient
//    {
//        public static string LastErrorMessage { get; private set; }

//        public static async Task<TResult> PostAsync<TResult, TInput>(string url, TInput model, IDictionary<string, string> headers, bool requireAuth = false, bool patchHttpMethod = false) where TResult : class
//        {
//            RestResponse response = null;
//            LastErrorMessage = null;
//            try
//            {
//                ZatcaLogger.Log("Zatca Http Client Started");
//                if (string.IsNullOrEmpty(SharedData.APIUrl))
//                {
//                    LastErrorMessage = "API URL is Null";
//                    return null;
//                }
//                if (requireAuth && (string.IsNullOrEmpty(SharedData.UserName) || string.IsNullOrEmpty(SharedData.Secret)))
//                {
//                    LastErrorMessage = "User Name Or Secret is Null";
//                    return null;
//                }
//                ZatcaLogger.Log(LastErrorMessage);

//                ServicePointManager.Expect100Continue = true;
//               // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
//                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 |SecurityProtocolType.Tls13 | SecurityProtocolType.SystemDefault ;

//                var client = new RestClient(SharedData.APIUrl);
//                var req = new RestRequest(url);
//                req.AddHeader("Accept-Version", "V2");
//                req.AddHeader("Accept-Language", "en");

//                req.AddHeader("Accept", "application/json");

//                if (headers != null)
//                {
//                    foreach (var header in headers)
//                    {
//                        req.AddHeader(header.Key, header.Value);
//                    }
//                }

//                if (requireAuth)
//                {
//                    req.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{SharedData.UserName}:{SharedData.Secret}")));
//                }

//                if (patchHttpMethod)
//                {
//                    req.RequestFormat = DataFormat.Json;
//                    req.AddBody(model);
//                    req.Method = Method.Patch;
//                    response = await client.ExecuteAsync(req);
//                }
//                else
//                {
//                    req.Method = Method.Post;
//                    req.RequestFormat = DataFormat.Json;
//                    req.AddBody(model);
//                    response = await client.ExecuteAsync(req);
//                }

//                //return result;
//                if (response.StatusCode == System.Net.HttpStatusCode.SeeOther 
//                    || response.StatusCode == System.Net.HttpStatusCode.InternalServerError
//                    || response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
//                    || (model is InputCSIDModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest)
//                    || (model is InputComplianceModel && response.StatusCode == System.Net.HttpStatusCode.BadRequest))
//                {
//                    LastErrorMessage = $"Response: {(int)response.StatusCode}-{response.StatusDescription} , Message : {response.Content}";
//                    ZatcaLogger.Log(LastErrorMessage);
//                    return null;
//                }

//                if (response.Content != null)
//                {
//                    var result = JsonConvert.DeserializeObject<TResult>(response.Content);

//                    var reportingResult = result as InvoiceModelResult;
//                    if (reportingResult != null)
//                    {
//                        if (reportingResult.ValidationResults?.ErrorMessages?.Count > 0 || reportingResult.ValidationResults?.WarningMessages?.Count > 0)
//                        {
//                            LastErrorMessage = JsonConvert.SerializeObject(reportingResult);
//                            ZatcaLogger.Log(LastErrorMessage);
//                        }
//                    }
//                    ZatcaLogger.Log("Zatca Http Client End Successfully!.");
//                    return result; 
//                }
//                LastErrorMessage = response.ErrorMessage + " : " + response.ErrorException.InnerException.Message ;
//                return null;
//            }
//            catch (Exception ex)
//            {
//                if (response != null)
//                {
//                    var errorRespone = response.Content;
//                    if (!string.IsNullOrEmpty(errorRespone))
//                    {
//                        LastErrorMessage = $" {response.StatusCode} : {response.ErrorMessage}";
//                    }
//                }
//                LastErrorMessage += ex.Message + " " + ex.InnerException?.Message;
//                ZatcaLogger.Log(LastErrorMessage);
//                return null;
//            }
//        }
//    }
//}
