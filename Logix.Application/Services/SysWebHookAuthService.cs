using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Logix.Application.Common;
using Logix.Application.DTOs.Integra;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Integra;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Logix.Application.Services.Main
{
    public class RequestParametersWebHookAuth
    {
        public long ID { get; set; }
        public string? URL { get; set; }
        public string? QueryAfterResult { get; set; }
        public string? Parameters { get; set; }
        public string? Method { get; set; }
        public string? Body { get; set; }
        public string? Headers { get; set; }
        public long? App_ID { get; set; }
        public long? USER_ID { get; set; }
        public long? Facility_ID { get; set; }
        public bool? IsSecurityProtocol { get; set; }
        public bool? IsSuccessCodeInBody { get; set; }
        public string? PathSuccessCode { get; set; }
        public string? SuccessCode { get; set; }
    }

    public class SysWebHookAuthService : GenericQueryService<SysWebHookAuth, SysWebHookAuthDto, SysWebHookAuthVw>, ISysWebHookAuthService
    {
        private readonly IMapper mapper;
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IConfiguration configuration;

        public SysWebHookAuthService(IQueryRepository<SysWebHookAuth> queryRepository, IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization,
            IConfiguration _configuration
            ) : base(queryRepository, mapper)
        {
            this.mapper = mapper;
            this.mainRepositoryManager = mainRepositoryManager;
            this.session = session;
            this.localization = localization;
            configuration = _configuration;
        }

        public async Task<IResult<SysWebHookAuthDto>> Add(SysWebHookAuthDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysWebHookAuthDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var item = mapper.Map<SysWebHookAuth>(entity);

                item.Name = entity.Name;
                item.Description = entity.Description;
                item.Url = entity.Url;
                item.MethodType = entity.MethodType;
                item.IsSecurityProtocol = entity.IsSecurityProtocol;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                item.Header = entity.Header;
                item.Parameter = entity.Parameter;
                item.Body = entity.Body;
                item.SuccessCode = entity.SuccessCode;
                item.QueryAfterResult = entity.QueryAfterResult;
                item.AppId = entity.AppId;
                item.Query = entity.Query;
                item.QueryDetails = entity.QueryDetails;
                item.BodyDetails = entity.BodyDetails;
                item.CreatedBy = session.UserId;
                item.ModifiedBy = session.UserId;
                item.CreatedOn = DateTime.Now;
                item.ModifiedOn = null;
                item.FacilityId = (int)session.FacilityId;
                item.IsEnabled = false;
                if (entity.State == 1)
                    item.IsEnabled = true;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                if (entity.IsSuccessCodeInBody == true)
                {
                    item.PathSuccessCode = entity.PathSuccessCode;
                }
                var newEntity = await mainRepositoryManager.SysWebHookAuthRepository.AddAndReturn(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookAuthDto>.SuccessAsync(mapper.Map<SysWebHookAuthDto>(newEntity), localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<SysWebHookAuthDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await mainRepositoryManager.SysWebHookAuthRepository.GetById(Id);
                if (item == null)
                    return Result<SysWebHookAuthDto>.Fail(localization.GetMessagesResource("InCorrectId"));

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                mainRepositoryManager.SysWebHookAuthRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookAuthDto>.SuccessAsync(mapper.Map<SysWebHookAuthDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysWebHookAuthDto>.FailAsync($"EXP in Remove at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysWebHookAuthEditDto>> Update(SysWebHookAuthEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysWebHookAuthEditDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var item = await mainRepositoryManager.SysWebHookAuthRepository.GetById(entity.Id);

                if (item == null) return await Result<SysWebHookAuthEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                mapper.Map(entity, item);

                item.Name = entity.Name;
                item.Description = entity.Description;
                item.Url = entity.Url;
                item.MethodType = entity.MethodType;
                item.IsSecurityProtocol = entity.IsSecurityProtocol;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                item.Header = entity.Header;
                item.Parameter = entity.Parameter;
                item.Body = entity.Body;
                item.SuccessCode = entity.SuccessCode;
                item.QueryAfterResult = entity.QueryAfterResult;
                item.AppId = entity.AppId;
                item.Query = entity.Query;
                item.QueryDetails = entity.QueryDetails;
                item.BodyDetails = entity.BodyDetails;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.FacilityId = (int)session.FacilityId;
                item.IsEnabled = false;
                if (entity.State == 1)
                    item.IsEnabled = true;
                item.IsSuccessCodeInBody = entity.IsSuccessCodeInBody;
                if (entity.IsSuccessCodeInBody == true)
                {
                    item.PathSuccessCode = entity.PathSuccessCode;
                }

                mainRepositoryManager.SysWebHookAuthRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysWebHookAuthEditDto>.SuccessAsync(mapper.Map<SysWebHookAuthEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exc)
            {
                return await Result<SysWebHookAuthEditDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
    


        public void SendToWebHookAuthThread(string WebHook_Auth_ID, long USER_ID, long Facility_ID)
        {
            ThreadStart webhookDelegate = new ThreadStart(() => SendToWebHookAuth(WebHook_Auth_ID, USER_ID, Facility_ID));
            Thread webhookThread = new Thread(webhookDelegate);
            webhookThread.Start();
        }

        public bool SendToWebHookAuth(string WebHook_Auth_ID, long USER_ID, long Facility_ID, long Integra_System_ID = 0)
        {
            DataTable dt = new DataTable();
            using (SqlConnection objCnn = new SqlConnection(GetConnectionString()))
            {
                objCnn.Open();
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandText = "SELECT TOP 1 * FROM Sys_WebHook_Auth_VW WHERE IsDeleted = 0 AND IsEnabled = 1 " +
                                         "AND Facility_ID = @Facility_ID AND (Id = @WebHook_Auth_ID OR App_ID = @Integra_System_ID)";
                    objCmd.Parameters.Add(new SqlParameter("@WebHook_Auth_ID", WebHook_Auth_ID));
                    objCmd.Parameters.Add(new SqlParameter("@Integra_System_ID", Integra_System_ID));
                    objCmd.Parameters.Add(new SqlParameter("@Facility_ID", Facility_ID));
                    using (SqlDataReader myReader = objCmd.ExecuteReader())
                    {
                        dt.Load(myReader);
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    return Send(row, USER_ID);
                }
            }
            return false;
        }

        public string GetBodyDetails(DataRow rowD)
        {
            string Detail = "";
            DataTable dt = new DataTable();
            string QueryDetails = !Convert.IsDBNull(rowD["QueryDetails"]) ? rowD["QueryDetails"].ToString() : string.Empty;
            string BodyDetail = !Convert.IsDBNull(rowD["BodyDetails"]) ? rowD["BodyDetails"].ToString() : string.Empty;

            if (!string.IsNullOrEmpty(BodyDetail) && !string.IsNullOrEmpty(QueryDetails) && QueryDetails.Length > 0)
            {
                using (SqlConnection objCnn = new SqlConnection(GetConnectionString()))
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.Text;
                        objCmd.CommandText = QueryDetails;
                        using (SqlDataReader myReader = objCmd.ExecuteReader())
                        {
                            dt.Load(myReader);
                        }
                    }
                }

                string body = "";
                int Count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DataTable dt2 = row.Table.Clone();
                    dt2.ImportRow(row);
                    Detail = ConvertToJson(BodyDetail, dt2);

                    body += Detail;
                    Count++;
                    if (Count < dt.Rows.Count)
                    {
                        body += ",";
                    }
                }
                BodyDetail = body;
            }
            return BodyDetail;
        }

        public bool Send(DataRow row, long USER_ID)
        {
            string Query = row["Query"] as string ?? " ";
            string qur = $"{Query}";
            RequestParametersWebHookAuth obj = new RequestParametersWebHookAuth();

            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection objCnn = new SqlConnection(GetConnectionString()))
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.Text;
                        objCmd.CommandText = qur;
                        using (SqlDataReader myReader = objCmd.ExecuteReader())
                        {
                            dt.Load(myReader);
                        }
                    }
                }

                string BodyDetails = GetBodyDetails(row);
                obj.ID = (long)row[nameof(obj.ID)];
                obj.URL = ConvertToJson(row[nameof(obj.URL)].ToString(), dt);
                obj.Parameters = ConvertToJson(row["Parameter"].ToString(), dt);
                obj.Method = ConvertToJson(row["MethodName"].ToString(), dt);
                obj.Headers = ConvertToJson(row["Header"].ToString(), dt);
                obj.App_ID = (long)row[nameof(obj.App_ID)];

                if (!string.IsNullOrEmpty(BodyDetails) && BodyDetails.Length > 0)
                {
                    dt.Columns.Add("BODYDETAILS", typeof(string));
                    foreach (DataRow rowM in dt.Rows)
                    {
                        rowM["BODYDETAILS"] = BodyDetails;
                    }
                }

                obj.Body = ConvertToJson(row[nameof(obj.Body)].ToString(), dt);
                obj.Facility_ID = Convert.ToInt32(row[nameof(obj.Facility_ID)]);
                obj.USER_ID = USER_ID;
                obj.IsSecurityProtocol = (bool)row[nameof(obj.IsSecurityProtocol)];
                obj.IsSuccessCodeInBody = (bool)row[nameof(obj.IsSuccessCodeInBody)];
                obj.PathSuccessCode = row[nameof(obj.PathSuccessCode)].ToString();
                obj.SuccessCode = row[nameof(obj.SuccessCode)].ToString();
                obj.QueryAfterResult = row[nameof(obj.QueryAfterResult)].ToString();

                //WebHookMethodsAuth webHookAuth = new WebHookMethodsAuth();
                //return webHookAuth.SendAPIs(obj);
            }
            catch (Exception)
            {
                // Handle exception (log or rethrow)
            }

            return false;
        }

        public string ConvertToJson(string dataweb, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string pattern = @"\(([^)]+)\)";
                MatchCollection matches = Regex.Matches(dataweb, pattern);
                Dictionary<string, string> columnMappings = new Dictionary<string, string>();

                foreach (Match match in matches)
                {
                    string columnName = match.Groups[1].Value.Trim();
                    if (dt.Columns.Contains(columnName))
                    {
                        object columnValue = dt.Rows[0][columnName];
                        string columnMappedValue = columnValue.ToString();

                        if (!columnMappings.ContainsKey(match.Value))
                        {
                            columnMappings.Add(match.Value, columnMappedValue);
                        }
                    }
                }

                foreach (var kvp in columnMappings)
                {
                    dataweb = dataweb.Replace(kvp.Key, kvp.Value);
                }
            }
            return dataweb;
        }

        private string GetConnectionString()
        {
            // Implement logic to retrieve the connection string.
            return configuration.GetConnectionString("LogixLocal");
        }
    

}
}
