using AutoMapper;
using Castle.Windsor.Installer;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.PUR;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Logix.Application.DTOs.WF;
using static SkiaSharp.HarfBuzz.SKShaper;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore;
using System.Linq;
using System.Text.Json.Nodes;

namespace Logix.Application.Services.Main
{
    public class SysRecordWebhookService : GenericQueryService<SysRecordWebhook, SysRecordWebhookDto, SysRecordWebhookVw>, ISysRecordWebhookService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IConfiguration configuration;

        public SysRecordWebhookService(IQueryRepository<SysRecordWebhook> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization,
            IConfiguration _configuration
            ) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
            configuration = _configuration;
        }

        public async Task<IResult<SysRecordWebhookDto>> Add(SysRecordWebhookDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysRecordWebhookDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var item = _mapper.Map<SysRecordWebhook>(entity);

                item.WebHookId = entity.WebHookId;
                item.ErrorCode = entity.ErrorCode;
                item.ErrorReason = entity.ErrorReason;
                item.Data = entity.Data;
                item.IsSended = entity.IsSended;
                item.LinkPage = entity.LinkPage;
                item.ReferenceId = entity.ReferenceId;
                item.IsDeleted = entity.IsDeleted;
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysRecordWebhookRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookDto>.SuccessAsync(_mapper.Map<SysRecordWebhookDto>(newEntity), localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<SysRecordWebhookDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysRecordWebhookEditDto>> Update(SysRecordWebhookEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysRecordWebhookEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysRecordWebhookRepository.GetById(entity.Id);

                if (item == null) return await Result<SysRecordWebhookEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.ErrorReason = entity.ErrorReason;
                item.ErrorCode = entity.ErrorCode;
                item.IsSended = entity.IsSended;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysRecordWebhookRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookEditDto>.SuccessAsync(_mapper.Map<SysRecordWebhookEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysRecordWebhookEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysRecordWebhookRepository.GetById(Id);
                if (item == null)
                    return Result<SysRecordWebhookDto>.Fail(localization.GetMessagesResource("InCorrectId"));

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                _mainRepositoryManager.SysRecordWebhookRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookDto>.SuccessAsync(_mapper.Map<SysRecordWebhookDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysRecordWebhookDto>.FailAsync($"EXP in Remove at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<List<SysRecordWebhookDto>>> RemoveSelectedItems(string stringIds, CancellationToken cancellationToken = default)
        {
            if (stringIds == null) return await Result<List<SysRecordWebhookDto>>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));

            try
            {
                var ids = stringIds.Split(',').Select(long.Parse).ToList();
                var items = await _mainRepositoryManager.SysRecordWebhookRepository.GetAll(
                    x => x.IsDeleted == false && ids.Contains(x.Id));
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        item.ModifiedOn = DateTime.Now;
                        item.ModifiedBy = session.UserId;
                        item.IsDeleted = true;
                    }
                    _mainRepositoryManager.SysRecordWebhookRepository.UpdateAll(items);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    return await Result<List<SysRecordWebhookDto>>.SuccessAsync(_mapper.Map<List<SysRecordWebhookDto>>(items), localization.GetResource1("UpdateSuccess"));
                }
                return await Result<List<SysRecordWebhookDto>>.FailAsync();
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Error in Update: {exp.Message} {(exp.InnerException != null ? "Inner: " + exp.InnerException.Message : "")}");
                return await Result<List<SysRecordWebhookDto>>.FailAsync($"Error during update: {exp.Message}");
            }
        }


        //public async Task SendWebhookData(string Id, long ScreenId, long UserId, long FacilityId, ProcessType PrType, bool? NoAuth = false)
        //{
        //    // Use the injected SysWebHookService instance
        //    await sys.SendToWebHooks(
        //        Id,
        //        ScreenId,
        //        UserId,
        //        FacilityId,
        //        (ProcessType)PrType,
        //        true);
        //}
        //public async Task<IResult<List<SysRecordWebhookDto>>> ReSendAPIs(string stringIds, long FacilityId, CancellationToken cancellationToken = default)
        //{
        //    if (stringIds == null) return await Result<List<SysRecordWebhookDto>>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));

        //    try
        //    {
        //        var ids = stringIds.Split(',').Select(long.Parse).ToList();
        //        //var getRecordWebHooksFromView = await _mainRepositoryManager.SysRecordWebhookRepository.GetAllFromViews(
        //        //    stringIds);

        //        var getRecordWebHooks = await _mainRepositoryManager.SysRecordWebhookRepository.GetAll(
        //            x => x.IsDeleted == false && ids.Contains(x.Id));

        //        if (getRecordWebHooks.Any())
        //        {
        //            foreach (var item in getRecordWebHooks)
        //            {
        //                await ReSendAPIs(item.Id, FacilityId);
        //            }
        //        }
        //        return await Result<List<SysRecordWebhookDto>>.FailAsync();
        //    }
        //    catch (Exception exc)
        //    {
        //        return await Result<List<SysRecordWebhookDto>>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
        //    }
        //}

        //private async Task<IResult<List<SysRecordWebhookDto>>> ReSendAPIs(long recordId, long FacilityId, bool? NoAuth = false)
        //{
        //    if (recordId <= 0) return await Result<List<SysRecordWebhookDto>>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));

        //    try
        //    {
        //        var StatusCode = 200;
        //        var obj = new SysRecordWebhook();
        //        var re_obj = new SysRecordWebhook();


        //        var dt = await _mainRepositoryManager.SysRecordWebhookRepository.GetAllFromViews(recordId);

        //        var requestParams = new RequestParameters();

        //        if (dt.Data.Count() > 0)
        //        {
        //            if (dt.IsSended == false)
        //                try
        //                {

        //                    re_obj.Data = dt.Data.ToString();
        //                    var method = dt.MethodName.ToString() ?? "";
        //                    requestParams.IsSecurityProtocol = (bool)dt.IsSecurityProtocol;
        //                    requestParams.IsSuccessCodeInBody = (bool)dt.IsSuccessCodeInBody;
        //                    requestParams.SuccessCode = dt.SuccessCode.ToString();
        //                    requestParams.PathSuccessCode = dt.PathSuccessCode.ToString();
        //                    requestParams.ReferenceId = dt.ReferenceId.ToString();
        //                    requestParams.ScreenId = dt.ScreenId;
        //                    requestParams.FacilityId = (long)dt.FacilityId;
        //                    requestParams.PrType = (int)dt.ProcessId;
        //                    requestParams.UserId = session.UserId;
        //                    requestParams.IsResendNewData = (bool)dt.IsResendNewData;
        //                    if (requestParams.IsResendNewData == true)
        //                    {

        //                        await sys.SendToWebHooks(requestParams.ReferenceId, requestParams.ScreenId, requestParams.UserId, requestParams.FacilityId, (ProcessType)requestParams.PrType, true);
        //                        obj.ErrorReason = "تم حذف السجل نظراً لاعادة ارسالة مره اخرى بالبيانات الحديثة" + obj.ErrorReason;
        //                        obj.ModifiedBy = session.UserId;
        //                        obj.IsDeleted = false;
        //                        _mainRepositoryManager.SysRecordWebhookRepository.Update(obj);
        //                        await _mainRepositoryManager.UnitOfWork.CompleteAsync();
        //                    }



        //                    if (requestParams.IsSecurityProtocol)
        //                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;


        //                    var requestData = re_obj.Data.ToString();

        //                    var lines = requestData.ToString();

        //                    var url = "";
        //                    var headers = "";
        //                    var body = "";
        //                    int Process_ID;
        //                    if (!string.IsNullOrEmpty(requestData))
        //                    {
        //                        //JObject json = JObject.Parse(requestData);
        //                        Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(requestData);

        //                        Dictionary<string, object> Dheaders = json.ToObject<Dictionary<string, object>>();

        //                        foreach (var dheader in Dheaders)
        //                        {
        //                            if (dheader.Key == "URL")
        //                            {
        //                                url = dheader.Value.ToString();
        //                            }
        //                            if (dheader.Key == "Headers")
        //                            {
        //                                headers = dheader.Value.ToString();
        //                            }
        //                            if (dheader.Key == "Body")
        //                            {
        //                                body = dheader.Value.ToString();
        //                            }
        //                            if (dheader.Key == "Process_ID")
        //                                Process_ID = (int)dheader.Value;
        //                        }
        //                    }

        //                    body = body.Trim();

        //                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        //                    req.Timeout = 60000 * 10;
        //                    req.ContentType = "application/json";
        //                    switch (method)
        //                    {
        //                        case "POST":
        //                            req.Method = "POST";
        //                            break;
        //                        case "GET":
        //                            req.Method = "GET";
        //                            break;
        //                        case "PUT":
        //                            req.Method = "PUT";
        //                            break;
        //                        case "DELETE":
        //                            req.Method = "DELETE";
        //                            break;
        //                        case "PATCH":
        //                            req.Method = "PATCH";
        //                            break;
        //                        case "HEAD":
        //                            req.Method = "HEAD";
        //                            break;
        //                        case "OPTIONS":
        //                            req.Method = "OPTIONS";
        //                            break;
        //                        default:
        //                            throw new Exception("Unsupported HTTP method");
        //                    }
        //                    if (!string.IsNullOrEmpty(headers))
        //                    {
        //                        Dictionary<string, object> Dheaders = JsonConvert.DeserializeObject<Dictionary<string, object>>(headers);

        //                        foreach (var Dheader in Dheaders)
        //                        {
        //                            if (Dheader.Key.ToLower() == "accept")
        //                                req.Accept = (string)Dheader.Value;
        //                            else if (Dheader.Key.ToLower() == "contenttype" || Dheader.Key.ToLower() == "content-type")
        //                                req.ContentType = (string)Dheader.Value;
        //                            else
        //                                req.Headers.Add(Dheader.Key, Dheader.Value.ToString());
        //                        }
        //                    }
        //                    if (!string.IsNullOrEmpty(body))
        //                    {
        //                        switch (req.ContentType.ToLower())
        //                        {
        //                            case "application/x-www-form-urlencoded":
        //                                {
        //                                    var jsonObject = JsonConvert.DeserializeObject<JObject>(body);
        //                                    var formData = ConvertJsonToFormUrlEncoded(jsonObject.ToString());

        //                                    byte[] byteArray = Encoding.UTF8.GetBytes(formData);
        //                                    req.ContentLength = byteArray.Length;

        //                                    using (Stream requestStream = req.GetRequestStream())
        //                                    {
        //                                        requestStream.Write(byteArray, 0, byteArray.Length);
        //                                    }
        //                                    break;
        //                                }

        //                            default:
        //                                {
        //                                    // For other content types, assume binary data
        //                                    byte[] data = Encoding.UTF8.GetBytes(body);
        //                                    req.ContentLength = data.Length;

        //                                    using (Stream requestStream = req.GetRequestStream())
        //                                    {
        //                                        requestStream.Write(data, 0, data.Length);
        //                                    }
        //                                    break;
        //                                }
        //                        }
        //                    }

        //                    using (var response = req.GetResponse())
        //                    {
        //                        using (var reader = new StreamReader(response.GetResponseStream()))
        //                        {
        //                            string responseBody = reader.ReadToEnd();
        //                            JObject jsonResponse;

        //                            try
        //                            {
        //                                jsonResponse = JObject.Parse(responseBody);
        //                            }
        //                            catch (Exception)
        //                            {
        //                                jsonResponse = null;
        //                            }

        //                            if (string.IsNullOrEmpty(requestParams.SuccessCode))
        //                            {
        //                                if (requestParams.SuccessCode.ToString().Length == 0)
        //                                {
        //                                    requestParams.SuccessCode = "200"; // Assuming SuccessCode is a string
        //                                }
        //                            }

        //                            bool IsSuccessCode = false;

        //                            if (requestParams.IsSuccessCodeInBody)
        //                            {
        //                                try
        //                                {
        //                                    JToken token = jsonResponse?.SelectToken(requestParams.PathSuccessCode);
        //                                    if (token != null)
        //                                    {
        //                                        string status = token.ToString();
        //                                        if (requestParams.SuccessCode == status)
        //                                        {
        //                                            IsSuccessCode = true;
        //                                        }
        //                                    }
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    // Handle exception if needed
        //                                }
        //                            }

        //                            var statusCode = response.GetType().GetProperty("StatusCode").GetValue(response, null);

        //                            if (!requestParams.IsSuccessCodeInBody &&
        //                                response.GetType().GetProperty("StatusCode").GetValue(response, null).ToString() == requestParams.SuccessCode)
        //                            {
        //                                obj.IsSended = true;
        //                            }

        //                            if (IsSuccessCode)
        //                            {
        //                                obj.IsSended = true;
        //                            }

        //                            if (!string.IsNullOrEmpty(responseBody))
        //                            {
        //                                obj.ErrorReason = string.IsNullOrEmpty(responseBody) ? "" : responseBody;
        //                            }

        //                            obj.ErrorCode = $"{response.GetType().GetProperty("StatusDescription").GetValue(response, null)} " +
        //                                            $"{response.GetType().GetProperty("StatusCode").GetValue(response, null)}";
        //                        }
        //                    }


        //                }
        //                catch (WebException ex)
        //                {
        //                    if (!string.IsNullOrEmpty(obj.ErrorReason))
        //                    {
        //                        obj.ErrorReason += ex.Message;
        //                    }

        //                    if (string.IsNullOrEmpty(obj.ErrorReason))
        //                    {
        //                        obj.ErrorReason = ex.Message;
        //                    }

        //                    using (HttpWebResponse errorResponse = ex.Response as HttpWebResponse)
        //                    {
        //                        try
        //                        {
        //                            if (errorResponse != null)
        //                            {
        //                                StatusCode = (int)errorResponse.StatusCode;
        //                                obj.ErrorCode = errorResponse.StatusCode.ToString();

        //                                using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
        //                                {
        //                                    string responseBody = reader.ReadToEnd();
        //                                    obj.ErrorReason += " Body: " + responseBody;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception exx)
        //                        {
        //                            obj.ErrorReason += exx.Message;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (!string.IsNullOrEmpty(obj.ErrorReason))
        //                    {
        //                        obj.ErrorReason += ex.Message;
        //                    }

        //                    if (string.IsNullOrEmpty(obj.ErrorReason))
        //                    {
        //                        obj.ErrorReason = ex.Message;
        //                    }

        //                    // return "Error: " + ex.Message;
        //                }

        //            if (StatusCode == (int)HttpStatusCode.Unauthorized && NoAuth == false && requestParams.IsResendNewData == true)
        //            {
        //                if (requestParams.AuthId != 0)
        //                {
        //                    //var whAuth = new ();
        //                    //if (!sysAuth.SendToWebHookAuth(requestParams.AuthId.ToString(), session.UserId, session.FacilityId))
        //                    //{
        //                    //    sys.SendToWebHooks(requestParams.ReferenceId, requestParams.ScreenId, requestParams.UserId, requestParams.FacilityId, (ProcessType)requestParams.PrType, true);
        //                    //    obj.ErrorReason = "تم حذف السجل نظراً لاعادة ارسالة مره اخراء بالبيانات الاخيرة" + obj.ErrorReason;
        //                    //    obj.ModifiedBy = session.UserId;
        //                    //    obj.IsDeleted = false;
        //                    //    _mainRepositoryManager.SysRecordWebhookRepository.Update(obj);
        //                    //    await _mainRepositoryManager.UnitOfWork.CompleteAsync();
        //                    //}
        //                }
        //            }

        //            obj.ModifiedBy = session.UserId;
        //            _mainRepositoryManager.SysRecordWebhookRepository.Update(obj);
        //            await _mainRepositoryManager.UnitOfWork.CompleteAsync();
        //        }

        //        return await Result<List<SysRecordWebhookDto>>.FailAsync();
        //    }
        //    catch (Exception exc)
        //    {
        //        return await Result<List<SysRecordWebhookDto>>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
        //    }
        //}

        //private string ConvertJsonToFormUrlEncoded(string jsonBody)
        //{
        //    var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonBody);

        //    var formData = new StringBuilder();
        //    foreach (var prop in jsonObject.Properties())
        //    {
        //        if (formData.Length > 0)
        //            formData.Append("&");
        //        formData.Append(Uri.EscapeDataString(prop.Name));
        //        formData.Append("=");
        //        formData.Append(Uri.EscapeDataString(prop.Value.ToString()));
        //    }

        //    return formData.ToString();
        //}
    }
}
