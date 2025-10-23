using Castle.Windsor.Installer;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using System.Data;
using System.Data.SqlClient;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysSmsRepository : GenericRepository<SysSms>, ISysSmsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public SysSmsRepository(ApplicationDbContext context,
            IConfiguration configuration) : base(context)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<int> AddByProcedure(SysSmsDto entity)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "[Sys_SMS_SP]";

                    objCmd.Parameters.AddWithValue("@ID", entity.Id);
                    objCmd.Parameters.AddWithValue("@Message", entity.Message);
                    objCmd.Parameters.AddWithValue("@Receiver_Mobile", entity.ReceiverMobile);
                    objCmd.Parameters.AddWithValue("@Facility_ID", entity.FacilityId);
                    objCmd.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 1);
                    objCmd.CommandTimeout = 600;
                    await objCnn.OpenAsync();
                    var obj = await objCmd.ExecuteScalarAsync();

                    return Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
