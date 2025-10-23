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
    public class SysCurrencyProfile : Profile
    {
        public SysCurrencyProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCurrencyDto, SysCurrency>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysCurrencyEditDto, SysCurrency>().ReverseMap();
        }
    }
    
}
