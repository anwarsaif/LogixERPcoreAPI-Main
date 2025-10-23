using AutoMapper;
using Castle.Windsor.Installer;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Castle.MicroKernel.ModelBuilder.Descriptors.InterceptorDescriptor;
using Logix.Application.DTOs.Integra;
using Logix.Domain.Integra;
using Logix.Application.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using Logix.Application.DTOs.PUR;

namespace Logix.Application.Services.Main
{
    //public class RequestParameters
    //{
    //    public long ID { get; set; }
    //    public string? WebhookURL { get; set; }
    //    public string? Parameters { get; set; }
    //    public string? Method { get; set; }
    //    public string? Body { get; set; }
    //    public string? Headers { get; set; }
    //    public long ProcessId { get; set; }
    //    public long UserId { get; set; }
    //    public long FacilityId { get; set; }
    //    public bool IsSecurityProtocol { get; set; }
    //    public bool IsSuccessCodeInBody { get; set; }
    //    public string? PathSuccessCode { get; set; }
    //    public string? SuccessCode { get; set; }
    //}
    public class RequestParameters
    {
        public long ID { get; set; }
        public string? WebhookURL { get; set; }
        public string? Parameters { get; set; }
        public string? Method { get; set; }
        public string? Body { get; set; }
        public string? Headers { get; set; }
        public long ProcessId { get; set; }
        public long UserId { get; set; }
        public long FacilityId { get; set; }
        public bool IsSecurityProtocol { get; set; }
        public bool IsSuccessCodeInBody { get; set; }
        public string? PathSuccessCode { get; set; }
        public string? SuccessCode { get; set; }
        public string? LinkPage { get; set; }
        public long AuthId { get; set; }
        public long AppId { get; set; }
        public string? ReferenceId { get; set; }
        public long ScreenId { get; set; }
        public int PrType { get; set; }
        public bool IsResendNewData { get; set; }
    }

    public class SysWebHookService : GenericQueryService<SysWebHook, SysWebHookDto, SysWebHookVw>, ISysWebHookService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysWebHookService(IQueryRepository<SysWebHook> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            IConfiguration configuration,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.configuration = configuration;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysWebHookDto>> Add(SysWebHookDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysWebHookDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var item = _mapper.Map<SysWebHook>(entity);

                item.Name = entity.Name;
                item.Description = entity.Description;
                item.Url = entity.Url;
                item.MethodType = entity.MethodType;
                item.Header = entity.Header;
                item.Parameter = entity.Parameter;
                item.Body = entity.Body;
                item.ProcessId = entity.ProcessId;
                item.AppId = entity.AppId;
                item.AuthId = entity.AuthId;
                item.LinkPage = entity.LinkPage;
                item.IsResendNewData = entity.IsResendNewData;
                item.FacilityId = (int)session.FacilityId;
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                item.IsEnabled = false;
                if (entity.Status == 1)
                    item.IsEnabled = true;

                item.ScreenId = entity.ScreenId;
                item.Query = entity.Query;
                item.QueryDetails = entity.QueryDetails;
                item.BodyDetails = entity.BodyDetails;
                item.IsSecurityProtocol = entity.IsSecurityProtocol;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                if (entity.IsSuccessCodeInBody == true)
                    item.PathSuccessCode = entity.PathSuccessCode;

                item.SuccessCode = entity.SuccessCode;

                var newEntity = await _mainRepositoryManager.SysWebHookRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookDto>.SuccessAsync(_mapper.Map<SysWebHookDto>(newEntity), localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<SysWebHookDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysWebHookEditDto>> Update(SysWebHookEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysWebHookEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysWebHookRepository.GetById(entity.Id);

                if (item == null) return await Result<SysWebHookEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.Name = entity.Name;
                item.Description = entity.Description;
                item.Url = entity.Url;
                item.MethodType = entity.MethodType;
                item.Header = entity.Header;
                item.Parameter = entity.Parameter;
                item.Body = entity.Body;
                item.ProcessId = entity.ProcessId;
                item.AppId = entity.AppId;
                item.AuthId = entity.AuthId;
                item.LinkPage = entity.LinkPage;
                item.IsResendNewData = entity.IsResendNewData;
                item.FacilityId = (int)session.FacilityId;
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                item.IsEnabled = false;
                if (entity.Status == 1)
                    item.IsEnabled = true;

                item.ScreenId = entity.ScreenId;
                item.Query = entity.Query;
                item.QueryDetails = entity.QueryDetails;
                item.BodyDetails = entity.BodyDetails;
                item.IsSecurityProtocol = entity.IsSecurityProtocol;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                if (entity.IsSuccessCodeInBody == true)
                    item.PathSuccessCode = entity.PathSuccessCode;

                item.SuccessCode = entity.SuccessCode;

                _mainRepositoryManager.SysWebHookRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookEditDto>.SuccessAsync(_mapper.Map<SysWebHookEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysWebHookEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysWebHookRepository.GetById(Id);
                if (item == null)
                    return Result<SysWebHookDto>.Fail(localization.GetMessagesResource("InCorrectId"));

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                _mainRepositoryManager.SysWebHookRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookDto>.SuccessAsync(_mapper.Map<SysWebHookDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysWebHookDto>.FailAsync($"EXP in Remove at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        public async Task<IResult<int>> SendToWebHook(string Id, long ScreenId, long UserId, long FacilityId, ProcessType PrType, CancellationToken cancellationToken = default)
        {
            try
            {
                var getWebHooks = await _mainRepositoryManager.SysWebHookRepository.GetAllFromView(h => h.IsDeleted == false && h.IsEnabled == true && h.FacilityId == FacilityId && h.ScreenId == ScreenId && h.ProcessId == (int)PrType);
                if (getWebHooks.Any())
                {
                    foreach (var item in getWebHooks)
                    {
                        await SendToWebHooks(Id,ScreenId, UserId, FacilityId, PrType);
                    }
                }
                return await Result<int>.FailAsync();
            }
            catch (Exception exc)
            {
                return await Result<int>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
        public async Task<IResult<int>> SendToWebHooks(string Id, long ScreenId, long UserId, long FacilityId, ProcessType PrType, bool? NoAuth = false)
        {

            try
            {
                int processId = (int)PrType;
                var obj = await _mainRepositoryManager.SysWebHookRepository.GetAllFromView(x => x.IsDeleted == false && x.IsEnabled == true && x.FacilityId == FacilityId && x.ScreenId == ScreenId && x.ProcessId == processId);
                if (obj.Count() == 0)
                {
                    foreach (var row in obj)
                        await Send(row, Id, UserId, ScreenId, PrType, NoAuth);
                }
                return await Result<int>.FailAsync();
            }
            catch (Exception exc)
            {
                return await Result<int>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
        public async Task Send(SysWebHookVw Webhook, string Id, long UserId, long Screen_ID, ProcessType PrType, bool? NoAuth = false)
        {
            try
            {
                string Query = Webhook.Query ?? " ";
                DataTable dt = new DataTable();

                using (var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", Id);
                        var dataReader = command.ExecuteReader();
                        dt.Load(dataReader);
                    }
                }

                string BodyDetails = GetBodyDetails(Webhook, Id);

                var obj = new RequestParameters();
                obj.ID = Webhook.Id;
                obj.WebhookURL = ConvertToJson(Webhook.Url ?? "", dt);
                obj.Parameters = ConvertToJson(Webhook.Parameter ?? "", dt);
                obj.Method = ConvertToJson(Webhook.MethodName ?? "", dt);
                obj.Headers = ConvertToJson(Webhook.Header ?? "", dt);
                obj.ProcessId = Convert.ToInt64(ConvertToJson(Webhook.ProcessId.ToString() ?? "", dt));

                //BodyDetails here

                obj.Body = ConvertToJson(Webhook.Body ?? "", dt);
                obj.FacilityId = Webhook.FacilityId ?? 0;
                obj.UserId = UserId;
                obj.IsSecurityProtocol = Webhook.IsSecurityProtocol ?? false;
                obj.IsSuccessCodeInBody = Webhook.IsSuccessCodeInBody ?? false;
                obj.PathSuccessCode = Webhook.PathSuccessCode;
                obj.SuccessCode = Webhook.SuccessCode;

                //send api
                SendAPIs(obj);
            }
            catch
            {
                throw;
            }
        }

        public string GetBodyDetails(SysWebHookVw Webhook, string Id)
        {
            string detail = "";
            string queryDetails = Webhook.QueryDetails.ToString() ?? "";
            string bodyDetails = Webhook.BodyDetails.ToString() ?? "";

            if (!string.IsNullOrEmpty(bodyDetails))
            {
                if (!string.IsNullOrEmpty(queryDetails))
                {
                    if (queryDetails.Length > 0)
                    {
                        using (var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                        {
                            connection.Open();

                            using (var command = connection.CreateCommand())
                            {
                                command.CommandType = CommandType.Text;
                                command.CommandText = queryDetails;
                                command.Parameters.AddWithValue("@ID", Id);

                                using (var reader = command.ExecuteReader())
                                {
                                    var dataTable = new DataTable();
                                    dataTable.Load(reader);

                                    var body = new StringBuilder(); // Use StringBuilder for efficient string concatenation
                                    for (int i = 0; i < dataTable.Rows.Count; i++)
                                    {
                                        var rowData = dataTable.Rows[i];
                                        var dataTableRow = rowData.Table.Clone();
                                        dataTableRow.ImportRow(rowData);
                                        detail = ConvertToJson(bodyDetails, dataTableRow);
                                        body.Append(detail);
                                        if (i < dataTable.Rows.Count - 1)
                                        {
                                            body.Append(",");
                                        }
                                    }
                                    bodyDetails = body.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return bodyDetails;
        }

        public string ConvertToJson(string dataweb, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                var matches = Regex.Matches(dataweb, @"\(([^)]+)\)"); // Use verbatim string for clarity
                var columnMappings = new Dictionary<string, string>();
                foreach (Match match in matches)
                {
                    var columnName = match.Groups[1].Value.Trim();
                    if (dt.Columns.Contains(columnName))
                    {
                        var columnValue = dt.Rows[0][columnName]?.ToString(); // Handle potential null values
                        if (!string.IsNullOrEmpty(columnValue))
                        {
                            columnMappings.Add(match.Value, columnValue);
                        }
                    }
                }

                // Replace column names with values in a single pass
                foreach (var kvp in columnMappings)
                {
                    dataweb = dataweb.Replace(kvp.Key, kvp.Value);
                }

                return dataweb;
            }

            return dataweb;
        }

        public async void SendAPIs(RequestParameters requestParams)
        {
            if (requestParams.IsSecurityProtocol)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }

            SysRecordWebhook obj = new SysRecordWebhook();
            string requestData = "";
            obj.WebHookId = requestParams.ID;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                string parameters = "";
                if (!string.IsNullOrEmpty(requestParams.Parameters))
                {
                    parameters = "?";
                    Dictionary<string, object> DParameter = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestParams.Parameters);
                    foreach (var _PParameter in DParameter)
                    {
                        parameters += $"{_PParameter.Key.ToString()}={_PParameter.Value.ToString()}&";
                    }
                }

                requestData = "{";
                string finalUrl = requestParams.WebhookURL.ToString();
                if (!string.IsNullOrEmpty(parameters))
                {
                    finalUrl = requestParams.WebhookURL + parameters;
                }

                requestData += $" \"URL\":  \"{finalUrl}\",";

                if (!string.IsNullOrEmpty(requestParams.Headers))
                {
                    requestData += $"\"Headers\":  {requestParams.Headers},";
                }

                byte[] data = null;

                if (!string.IsNullOrEmpty(requestParams.Body))
                {
                    requestData += $"\"Body\":  {requestParams.Body}, \"Process_ID\": {requestParams.ProcessId}" + "}";
                    string cleanedString = requestParams.Body.Replace("\r\n", "");
                    requestParams.Body = cleanedString;
                    data = Encoding.UTF8.GetBytes(requestParams.Body);
                }

                obj.Data = requestData;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(finalUrl);

                // Set the timeout to 60 seconds (60000 milliseconds)
                req.Timeout = 60000;

                req.ContentType = "application/json";

                switch (requestParams.Method)
                {
                    case "POST":
                        req.Method = "POST";
                        break;
                    case "GET":
                        req.Method = "GET";
                        break;
                    case "PUT":
                        req.Method = "PUT";
                        break;
                    case "DELETE":
                        req.Method = "DELETE";
                        break;
                    case "PATCH":
                        req.Method = "PATCH";
                        break;
                    case "HEAD":
                        req.Method = "HEAD";
                        break;
                    case "OPTIONS":
                        req.Method = "OPTIONS";
                        break;
                    default:
                        // Handle unsupported methods or defaults here
                        break;
                }

                if (!string.IsNullOrEmpty(requestParams.Headers))
                {
                    Dictionary<string, object> Dheaders = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestParams.Headers);
                    // Add headers to the request
                    foreach (var _headers in Dheaders)
                    {
                        req.Headers.Add(_headers.Key, _headers.Value.ToString());
                    }
                }

                if (data != null)
                {
                    req.ContentLength = data.Length;
                    using (Stream requestStream = req.GetRequestStream())
                    {
                        requestStream.Write(data, 0, data.Length);
                    }
                }

                using (var response = req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseBody = reader.ReadToEnd();
                        JObject jsonResponse = null;

                        try
                        {
                            jsonResponse = JObject.Parse(responseBody);
                        }
                        catch (Exception ex)
                        {
                            // Handle JSON parsing exception
                        }

                        if (string.IsNullOrEmpty(requestParams.SuccessCode))
                        {
                            if (requestParams.SuccessCode == "0")
                            {
                                requestParams.SuccessCode = "200";
                            }
                        }

                        bool isSuccessCode = false;

                        if (requestParams.IsSuccessCodeInBody)
                        {
                            try
                            {
                                JToken token = jsonResponse.SelectToken(requestParams.PathSuccessCode);

                                if (token != null)
                                {
                                    string status = token.ToString();

                                    if (requestParams.SuccessCode == status)
                                    {
                                        isSuccessCode = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle JSON parsing exception
                            }
                        }

                        if (requestParams.IsSuccessCodeInBody == false && response.GetType().GetProperty("StatusCode").GetValue(response, null).ToString() == requestParams.SuccessCode)
                        {
                            obj.IsSended = true;
                        }

                        if (isSuccessCode)
                        {
                            obj.IsSended = true;
                        }

                        if (jsonResponse != null)
                        {
                            obj.ErrorReason = string.IsNullOrEmpty(jsonResponse.ToString()) ? "" : jsonResponse.ToString();
                        }

                        obj.ErrorCode = response.GetType().GetProperty("StatusDescription").GetValue(response, null).ToString() + " " + response.GetType().GetProperty("StatusCode").GetValue(response, null).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(obj.ErrorReason))
                {
                    obj.ErrorReason += ex.Message;
                }

                if (string.IsNullOrEmpty(obj.ErrorReason))
                {
                    obj.ErrorReason = ex.Message;

                }
            }

            obj.CreatedBy = requestParams.UserId;
            var CreateSysRecordWebhook = await _mainRepositoryManager.SysRecordWebhookRepository.AddAndReturn(obj);
        }

    }
}
