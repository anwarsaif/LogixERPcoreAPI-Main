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
    public class SysWebHookAuthProfile : Profile
    {
        public SysWebHookAuthProfile() 
        {
            CreateMap<SysWebHookAuthDto, SysWebHookAuth>().ReverseMap();
            CreateMap<SysWebHookAuthEditDto, SysWebHookAuth>().ReverseMap();
        }
    }
}
