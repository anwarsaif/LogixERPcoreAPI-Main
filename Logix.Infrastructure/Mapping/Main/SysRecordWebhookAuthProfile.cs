using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysRecordWebhookAuthProfile : Profile
    {
        public SysRecordWebhookAuthProfile()
        {
            CreateMap<SysRecordWebhookAuthDto, SysRecordWebhookAuth>().ReverseMap();
            CreateMap<SysRecordWebhookAuthEditDto, SysRecordWebhookAuth>().ReverseMap();
        }
    }
}
