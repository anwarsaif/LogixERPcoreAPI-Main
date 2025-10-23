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
    public class SysCitesProfile: Profile
    {
        public SysCitesProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCitesDto, SysCites>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysCitesEditDto, SysCites>().ReverseMap();
        }
    }
}
