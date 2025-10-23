using Logix.Application.DTOs.Main;
using Logix.Application.Extensions;
using Logix.Application.Interfaces.IServices;
using Logix.Domain.ACC;
using Logix.Domain.Main;

namespace Logix.MVC.Helpers
{

    public interface IScreenPropertiesHelper
    {
        Task<string> GetValue(long propertyId, long userId = 0);
        Task<bool> IsAllowed(long propertyId, long userId = 0);
        Task<SysScreenPermissionPropertyDto> GetById(long propertyId, long userId = 0);
    }



    public class ScreenPropertiesHelper : IScreenPropertiesHelper
    {
        private readonly IMainServiceManager serviceManager;
        private readonly ISession _session;
        public ScreenPropertiesHelper(IMainServiceManager serviceManager, IHttpContextAccessor httpContextAccessor)
        {
            this.serviceManager = serviceManager;
            _session = httpContextAccessor.HttpContext.Session;
        }
        public async Task<string> GetValue(long propertyId, long userId = 0)
        {
            try
            {
                var user = _session.GetData<SysUser>("user");
                if (user == null)
                {
                    return string.Empty;
                }

                if (user.Id == 0)
                {
                    return string.Empty;
                }

                var get = await serviceManager.SysScreenPermissionPropertyService.GetByProperty(propertyId, user.Id);
                if (get.Succeeded && get.Data != null)
                {
                    return string.IsNullOrEmpty(get.Data.Value) ? string.Empty : get.Data.Value;
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

        public async Task<bool> IsAllowed(long propertyId, long userId = 0)
        {
            try
            {
                var user = _session.GetData<SysUser>("user");
                if (user == null)
                {
                    return false;
                }

                if (user.Id == 0)
                {
                    return false;
                }

                var get = await serviceManager.SysScreenPermissionPropertyService.GetByProperty(propertyId, user.Id);
                if (get.Succeeded && get.Data != null)
                {
                    return get.Data.Allow ?? false;
                }

                return false;
            }
            catch (Exception exp)
            {
                Console.WriteLine($"=== Exp in get of {this.GetType}, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")} .");
                throw exp;
                //return string.Empty;
            }
        }

        public async Task<SysScreenPermissionPropertyDto> GetById(long propertyId, long userId = 0)
        {
            try
            {
                var user = _session.GetData<SysUser>("user");
                if (user == null)
                {
                    return default;
                }

                if (user.Id == 0)
                {
                    return default;
                }

                var get = await serviceManager.SysScreenPermissionPropertyService.GetByProperty(propertyId, user.Id);
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
