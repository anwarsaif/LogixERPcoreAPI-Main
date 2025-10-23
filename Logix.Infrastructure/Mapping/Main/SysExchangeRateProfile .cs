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
    public class SysExchangeRateProfile:Profile
    {
        public SysExchangeRateProfile() {
            //Mapping CreateDto To Entity
            CreateMap<SysExchangeRateDto, SysExchangeRate>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysExchangeRateEditDto, SysExchangeRate>().ReverseMap();
        }
    }
}
