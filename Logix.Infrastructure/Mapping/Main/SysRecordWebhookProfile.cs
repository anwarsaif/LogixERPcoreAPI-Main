using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public partial class SysRecordWebhookProfile : Profile
    {
        public SysRecordWebhookProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysRecordWebhookDto, SysRecordWebhook>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysRecordWebhookEditDto, SysRecordWebhook>().ReverseMap();
            //CreateMap<SelectedItemIdsListDto, SysRecordWebhook>().ReverseMap();
        }
    }
}