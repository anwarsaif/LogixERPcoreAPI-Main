using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class InvestEmployeeRepository : GenericRepository<InvestEmployee>, IInvestEmployeeRepository
    {
        private readonly ApplicationDbContext context;

        public InvestEmployeeRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<Result<SelectList>> DDLFieldColumns()
        {
            try
            {
                var excluded = new[]
                {
            "ID", "Emp_ID", "ISDEL", "Status_ID",
            "IsDeleted", "Is_Sub", "Parent_ID"
        };

                var excludedColumns = string.Join(",", excluded.Select(e => $"'{e}'"));
                var sql = $@"
            SELECT COLUMN_NAME 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'INVEST_Employee' 
              AND COLUMN_NAME NOT IN ({excludedColumns})";

                var columns = new List<string>();

                using (var conn = context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                columns.Add(reader.GetString(0));
                            }
                        }
                    }
                }

                var list = new List<string>();
                list.AddRange(columns);

                // convert to SelectList
                var selectList = new SelectList(list);

                return await Result<SelectList>.SuccessAsync(selectList);
            }
            catch (Exception ex)
            {
                return await Result<SelectList>.FailAsync(ex.Message);
            }
        }

    }
}
