using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysScreenPermissionRepository : GenericRepository<SysScreenPermission>, ISysScreenPermissionRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly ICurrentData session;

        public SysScreenPermissionRepository(ApplicationDbContext context,
            IConfiguration configuration,
            ICurrentData session) : base(context)
        {
            this.context = context;
            this.configuration = configuration;
            this.session = session;
        }

        public async Task<SysScreenPermission> GetByScreenAndGroup(long screenId, int groupId)
        {
            return await context.SysScreenPermissions.Where(s => s.ScreenId == screenId && s.GroupId == groupId).AsNoTracking().SingleAsync();
        }

        public async Task<List<UserPermissionSearchVm>> GetUserPermissionReport(UserPermissionSearchVm obj)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "[SYS_USER_SP]";

                    objCmd.Parameters.AddWithValue("@Screen_Id", obj.ScreenId ?? 0);
                    objCmd.Parameters.AddWithValue("@USER_ID", obj.UserId);
                    objCmd.Parameters.AddWithValue("@System_Id", obj.SystemId);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 27);
                    objCmd.CommandTimeout = 600;
                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(result);

                    List<UserPermissionSearchVm> results = new List<UserPermissionSearchVm>();
                    foreach (DataRow row in dt.Rows)
                    {
                        results.Add(new UserPermissionSearchVm
                        {
                            UserName = row["USER_FULLNAME"].ToString(),
                            ScreenName = row["SCREEN_NAME"].ToString(),
                            ScreenName2 = row["SCREEN_NAME2"].ToString(),
                            SystemName = row["System_Name"].ToString(),

                            ScreenShow = Convert.ToBoolean(row["SCREEN_SHOW"]),
                            ScreenAdd = Convert.ToBoolean(row["SCREEN_ADD"]),
                            ScreenEdit = Convert.ToBoolean(row["SCREEN_EDIT"]),
                            ScreenDelete = Convert.ToBoolean(row["SCREEN_DELETE"]),
                            ScreenPrint = Convert.ToBoolean(row["SCREEN_PRINT"]),

                        });
                    }
                    return results;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PaginatedResult<List<UserPermissionSearchVm>>> GetUserPermissionReportAsync(UserPermissionSearchVm obj, int take = Pagination.take, int? lastSeenId = null)
        {
            try
            {
                using var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal"));
                using var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetScreenPermissionsWithPagination";

                // Parameters
                command.Parameters.AddWithValue("@USER_ID", obj.UserId ?? 0);
                command.Parameters.AddWithValue("@System_Id", obj.SystemId ?? 0);
                command.Parameters.AddWithValue("@Screen_Id", obj.ScreenId ?? 0);
                command.Parameters.AddWithValue("@LastSeenScreenId", lastSeenId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Take", take);
                command.CommandTimeout = 600;

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                // 1️⃣ قراءة البيانات
                var data = new List<UserPermissionSearchVm>();
                var dt = new DataTable();
                dt.Load(reader);

                foreach (DataRow row in dt.Rows)
                {
                    data.Add(new UserPermissionSearchVm
                    {
                        ScreenId = row["SCREEN_ID"] != DBNull.Value ? Convert.ToInt32(row["SCREEN_ID"]) : 0,
                        SystemId = row["System_Id"] != DBNull.Value ? Convert.ToInt32(row["System_Id"]) : 0,
                        UserName = row["USER_FULLNAME"]?.ToString(),
                        SystemName = row["System_Name"]?.ToString(),
                        ScreenShow = row["SCREEN_SHOW"] != DBNull.Value && Convert.ToBoolean(row["SCREEN_SHOW"]),
                        ScreenAdd = row["SCREEN_ADD"] != DBNull.Value && Convert.ToBoolean(row["SCREEN_ADD"]),
                        ScreenEdit = row["SCREEN_EDIT"] != DBNull.Value && Convert.ToBoolean(row["SCREEN_EDIT"]),
                        ScreenDelete = row["SCREEN_DELETE"] != DBNull.Value && Convert.ToBoolean(row["SCREEN_DELETE"]),
                        ScreenPrint = row["SCREEN_PRINT"] != DBNull.Value && Convert.ToBoolean(row["SCREEN_PRINT"])
                    });
                }


                // 2️⃣ قراءة PaginationInfo
                object paginationInfo = null;
                if (await reader.NextResultAsync())
                {
                    var metaTable = new DataTable();
                    metaTable.Load(reader);
                    if (metaTable.Rows.Count > 0)
                    {
                        var row = metaTable.Rows[0];
                        paginationInfo = new
                        {
                            lastSeenId = row["LastSeenScreenId"] != DBNull.Value ? Convert.ToInt64(row["LastSeenScreenId"]) : (long?)null,
                            pageSize = Convert.ToInt32(row["PageSize"]),
                            totalItems = Convert.ToInt32(row["TotalItems"]),
                            totalPages = Convert.ToInt32(row["TotalPages"]),
                            hasMore = Convert.ToBoolean(row["HasMore"])
                        };
                    }
                }

                // 3️⃣ إرجاع النتيجة في PaginatedResult
                return new PaginatedResult<List<UserPermissionSearchVm>>
                {
                    Succeeded = true,
                    Data = data,
                    Status = new Message { code = 200, message = "Records retrieved successfully" },
                    PaginationInfo = paginationInfo
                };
            }
            catch (Exception ex)
            {
                return PaginatedResult<List<UserPermissionSearchVm>>.Fail($"Error executing usp_GetScreenPermissionsWithPagination: {ex.Message}");
            }
        }


    }
}
