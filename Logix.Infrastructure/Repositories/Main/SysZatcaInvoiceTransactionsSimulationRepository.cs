using System.Data;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Logix.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Logix.Application.DTOs.Main;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysZatcaInvoiceTransactionsSimulationRepository : GenericRepository<SysZatcaInvoiceTransactionsSimulation>, ISysZatcaInvoiceTransactionsSimulationRepository
    {
        private readonly IConfiguration configuration;
        private readonly ICurrentData session;

        public SysZatcaInvoiceTransactionsSimulationRepository(ApplicationDbContext context,
            IConfiguration _configuration,
            ICurrentData session) : base(context)
        {
            this.configuration = _configuration;
            this.session = session;
        }

        public async Task<DataTable> GetTransactions_Simulation(ZatcaInvoiceFilterDto filter)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "ZatcaTransactions_Simulation";

                    objCmd.Parameters.AddWithValue("@Invoice_According_Type_ID", filter.InvoiceAccordingTypeId);
                    objCmd.Parameters.AddWithValue("@Facility_ID", session.FacilityId);
                    objCmd.Parameters.AddWithValue("@InvoiceStatus", filter.InvoiceStatus);
                    if (!string.IsNullOrEmpty(filter.Code))
                        objCmd.Parameters.AddWithValue("@Code", filter.Code);

                    if (!string.IsNullOrEmpty(filter.CustomerCode))
                        objCmd.Parameters.AddWithValue("@CustomerCode", filter.CustomerCode);
                    else if (!string.IsNullOrEmpty(filter.CustomerName) && session.Language == 1)
                        objCmd.Parameters.AddWithValue("@CustomerName", filter.CustomerName);
                    else if (!string.IsNullOrEmpty(filter.CustomerName) && session.Language == 2)
                        objCmd.Parameters.AddWithValue("@CustomerName2", filter.CustomerName);

                    if (filter.BranchId != 0)
                        objCmd.Parameters.AddWithValue("@Branch_ID", filter.BranchId);
                    else
                    {
                        objCmd.Parameters.AddWithValue("@Branch_ID", 0);
                        objCmd.Parameters.AddWithValue("@Branchs_ID", session.Branches);
                    }

                    if (filter.InvoiceType != 0)
                        objCmd.Parameters.AddWithValue("@InvoiceType", filter.InvoiceType);

                    if (filter.PaymentTermsId != 0)
                        objCmd.Parameters.AddWithValue("@Payment_Terms_ID", filter.PaymentTermsId);

                    if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate))
                    {
                        objCmd.Parameters.AddWithValue("@StartDate", filter.StartDate);
                        objCmd.Parameters.AddWithValue("@EndDate", filter.EndDate);
                    }

                    objCmd.Parameters.AddWithValue("@CMDTYPE", filter.InvoiceAccordingTypeId);
                    objCmd.CommandTimeout = 666666;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);
                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}