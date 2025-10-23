using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.Main;
using Logix.Application.Extensions;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Domain.ACC;

namespace Logix.Application.Helpers
{
    public interface ISysConfigurationAppHelper
    {
        Task<string> GetValue(long propertyId, long facilityId = 0);
        Task<SysPropertyValueDto> GetById(long propertyId, long facilityId = 0);
        Task<bool> CheckDate(string curDate);

    }



    public class SysConfigurationAppHelper : ISysConfigurationAppHelper
    {
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly ICurrentData _session;
        private readonly IMapper _mapper;

        public SysConfigurationAppHelper(IMainRepositoryManager mainRepositoryManager, ICurrentData session, IMapper _mapper)
        {
            this.mainRepositoryManager = mainRepositoryManager;
            _session = session;
            this._mapper = _mapper;
        }
        public async Task<string> GetValue(long propertyId, long facilityId = 0)
        {
            try
            {
                var fId = facilityId == 0 ? _session.FacilityId : facilityId;

                if (fId == 0)
                {
                    return string.Empty;
                }

                var get = await mainRepositoryManager.SysPropertyValueRepository.GetByProperty(propertyId, fId);
                if ( get != null)
                {
                    return string.IsNullOrEmpty(get.PropertyValue) ? string.Empty : get.PropertyValue;
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

                var get = await mainRepositoryManager.SysPropertyValueRepository.GetByProperty(propertyId, tempFacility);
                if (get != null)
                {
                    var entityMap = _mapper.Map<SysPropertyValueDto>(get);
                    return entityMap;
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

        public async Task<bool> CheckDate(string curDate)
        {
            bool ret = false;
            try
            {
                //------------------------------------نوع التقويم المعتمد
                string CalendarType = "0";
                var Calendar = await mainRepositoryManager.SysPropertyValueRepository.GetByProperty(19, _session.FacilityId);
                if (Calendar != null)
                {
                    CalendarType = Calendar.PropertyValue;
                }


                int year = int.Parse(curDate.Substring(0, 4));
                if (CalendarType == "1")
                {
                    if (year >= 1900 && year <= 2100)
                        ret = true;
                    else
                        return false;
                }
                else
                {
                    if (year >= 1300 && year <= 1500)
                        ret = true;
                    else
                        return false;
                }

                int month = int.Parse(curDate.Substring(5, 2));
                if (month < 1 || month > 12)
                    return false;

                int day = int.Parse(curDate.Substring(8, 2));
                if (day < 1 || day > 31)
                    return false;

                if (curDate[4] != '/' || curDate[7] != '/')
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid date specified", ex);
            }

            return ret;
        }
    }
}
