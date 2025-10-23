using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Domain.Main;

namespace Logix.Application.Helpers
{
    public interface ISendSmsHelper

    {
        Task<bool> SendSms(string PhoneNumbers, string Message, bool IsRepeat = false, bool IsArabic = false, long FacilityId = 0, int UserId = 0);
    }

    public class SendSmsHelper : ISendSmsHelper
    {
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly ICurrentData _session;
        private readonly HttpClient _httpClient;
        private readonly IMapper mapper;

        public SendSmsHelper(IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            HttpClient httpClient,
            IMapper mapper
            )
        {
            this.mainRepositoryManager = mainRepositoryManager;
            this._session = session;
            _httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task<bool> SendSms(string PhoneNumbers, string Message, bool IsRepeat = false, bool IsArabic = false, long FacilityId = 0, int UserId = 0)
        {
            string smsId = "";
            var sysSmsObj = new SysSmsDto
            {
                Message = Message,
                ReceiverMobile = PhoneNumbers
            };

            try
            {
                string apiUrl = "";

                if (FacilityId == 0)
                {
                    var getUrl = await mainRepositoryManager.SysPropertyValueRepository.GetByProperty(41, _session.FacilityId);
                    if (getUrl != null)
                        apiUrl = getUrl.PropertyValue ?? "";

                    sysSmsObj.FacilityId = _session.FacilityId;
                    sysSmsObj.CreatedBy = _session.UserId;
                }
                else
                {
                    var getUrl = await mainRepositoryManager.SysPropertyValueRepository.GetByProperty(41, FacilityId);
                    if (getUrl != null)
                        apiUrl = getUrl.PropertyValue ?? "";

                    sysSmsObj.FacilityId = FacilityId;
                    sysSmsObj.CreatedBy = UserId;
                }
                var MappedEntity = mapper.Map<SysSms>(sysSmsObj);

                var addSms = await mainRepositoryManager.SysSmsRepository.AddAndReturn(MappedEntity);
                await mainRepositoryManager.UnitOfWork.CompleteAsync();
                smsId = addSms != null ? addSms.Id.ToString() : "";

                string url = apiUrl.Replace("{message}", Message).Replace("{numbers}", PhoneNumbers);
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch
            {
                // want to continue
                //return false;
            }

            try
            {
                // var send = await mainRepositoryManager.SysWebHookRepository.SendToWebHook(smsId, 24, sysSmsObj.CreatedBy ?? 0, sysSmsObj.FacilityId ?? 0, ProcessType.Added);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}