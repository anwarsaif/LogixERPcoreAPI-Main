using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.WF;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Domain.SAL;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysDynamicAttributeRepository : GenericRepository<SysDynamicAttribute>, ISysDynamicAttributeRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;
        public SysDynamicAttributeRepository(ApplicationDbContext context, ICurrentData session) : base(context)
        {
            this.context = context;
            this.session = session;
        }

        public async Task<SysDynamicAttribute> RemoveUsingProcedure(long id)
        {
            var item = await context.SysDynamicAttributes.SingleOrDefaultAsync(t => t.Id == id);
            if (item != null)
            {
                item.IsDeleted= true;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                context.SaveChanges();
                return item;
            }
            throw new Exception("CustomException: No row affected");
        }

        public async Task<SysDynamicAttribute> UpdateUsingProcedure(SysDynamicAttributeEditDto obj)
        {
            var existingAttribute =  context.SysDynamicAttributes.Find(obj.DynamicAttributeId);
            if (existingAttribute != null)
            {
                existingAttribute.ScreenId = obj.ScreenId;
                existingAttribute.DataTypeId = obj.DataTypeId ?? 0;
                existingAttribute.AttributeName = obj.AttributeName;
                existingAttribute.SortOrder = obj.SortOrder ?? 0;
                existingAttribute.Required = obj.Required;
                existingAttribute.LookUpCatagoriesId = obj.LookUpCatagoriesId ?? 0;
                existingAttribute.ModifiedBy = session.UserId;
                existingAttribute.ModifiedOn = DateTime.Now;
                existingAttribute.DefaultValue = obj.DefaultValue;
                existingAttribute.MaxLength = obj.MaxLength;
                existingAttribute.TableId = obj.TableId ?? 0;
                existingAttribute.AttributeName2 = obj.AttributeName2;
                existingAttribute.StepId = obj.StepId ?? 0;

                context.SaveChanges();
                var newobj = await context.SysDynamicAttributes.SingleOrDefaultAsync(t => t.Id == obj.Id);
                return newobj;
            }
            throw new Exception("CustomException: No row affected");
        }





        public async Task<List<DynamicAttributeResult>> GetDynamicAttributeData(long screenId, long appId )
        {



            var query = from da in context.SysDynamicAttributes
                        where da.IsDeleted ==false && da.ScreenId==screenId
                        select new DynamicAttributeResult
                        {
                            ID = da.Id,
                            DynamicAttributeId = da.DynamicAttributeId,
                            Screen_ID = da.ScreenId,
                            DataTypeId = (DataTypeIdEnum)da.DataTypeId,
                            AttributeName = da.AttributeName,
                            SortOrder = da.SortOrder,
                            Required = da.Required,
                            LookUp_Catagories_ID = da.LookUpCatagoriesId,
                            DynamicValue = (from dv in context.SysDynamicValues
                                            where dv.AttributeId == da.DynamicAttributeId &&
                                                  dv.ApplicationId == appId
                                            select dv.DynamicValue).FirstOrDefault(),
                            AttributeName2 = da.AttributeName2
                        };

            return (List<DynamicAttributeResult>)query.ToList().OrderBy(da => da.SortOrder);
            


        }



    }
}
