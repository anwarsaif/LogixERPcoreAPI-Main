using Logix.Application.Common;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.Main;
using Logix.Application.Extensions;
using Logix.Application.Interfaces.IServices;
using Logix.Domain.ACC;

namespace Logix.MVC.Helpers
{
    public interface ISysConfigurationHelper
    {
        Task<string> GetValue(long propertyId, long facilityId=0);
        Task<SysPropertyValueDto> GetById(long propertyId, long facilityId=0);
    }



    public class SysConfigurationHelper : ISysConfigurationHelper
    {
        private readonly IMainServiceManager serviceManager;
        private readonly ICurrentData _session;
        public SysConfigurationHelper(IMainServiceManager serviceManager, ICurrentData session)
        {
            this.serviceManager = serviceManager;
            _session = session;
        }
        public async Task<string> GetValue(long propertyId, long facilityId=0)
        {
            try
            {
                var fId = facilityId==0?_session.FacilityId:facilityId;

                if (fId == 0)
                {
                    return string.Empty;
                }
                
                var get = await serviceManager.SysPropertyValueService.GetByProperty(propertyId, fId);
                if(get.Succeeded && get.Data != null)
                {
                    return string.IsNullOrEmpty(get.Data.PropertyValue)?string.Empty: get.Data.PropertyValue;
                }

                return string.Empty;
            }
            catch (Exception exp)
            {
                Console.WriteLine($"=== Exp in get of {this.GetType}, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")} .");
                throw exp;
                //return string.Empty;
            }
        }

        public async Task<SysPropertyValueDto> GetById(long propertyId, long facilityId = 0)
        {
            try
            {
                long tempFacility = facilityId;
                if (_session.FacilityId != 0)
                {
                    tempFacility = _session.FacilityId;
                    //return default;
                }

                var get = await serviceManager.SysPropertyValueService.GetByProperty(propertyId, tempFacility);
                if (get.Succeeded && get.Data != null)
                {
                    return get.Data;
                }

                return default;
            }
            catch (Exception exp)
            {
                Console.WriteLine($"=== Exp in get of {this.GetType}, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")} .");
                throw exp;
                //return string.Empty;
            }
        }
    }
}
