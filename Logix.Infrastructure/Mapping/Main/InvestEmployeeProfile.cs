using AutoMapper;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.HR.EmployeeDto;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class InvestEmployeeProfile : Profile
    {
        public InvestEmployeeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<InvestEmployeeDto, InvestEmployee>().ReverseMap();
            CreateMap<InvestEmployeeAddDto, InvestEmployee>().ReverseMap();
            CreateMap<InvestEmployeeAddDto, InvestEmployeeDto>().ReverseMap();

            // this dto for add new employee from employeescreen in main system
            CreateMap<InvestEmployeeDto2, InvestEmployee>().ReverseMap();
            CreateMap<InvestEmployeeDto2, InvestEmployeeDto>().ReverseMap();

            CreateMap<InvestEmployeeEditDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeFastAddDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeFastAddDto, InvestEmployeeDto>().ReverseMap();


            // mapping For Some Customes Dto
            CreateMap<EmployeeSubDto, InvestEmployee>().ReverseMap();

            // ========== mapping for edit dtos==========================            
            CreateMap<EmployeeAdditionalPropsDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeContactInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeContractInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeDependentsDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeFollowersDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeJobInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeMainInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeMedicalInsuranceDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeePreparingDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeSalaryInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeSocialInsuranceDto, InvestEmployee>().ReverseMap();
            CreateMap<EmployeeTravelInfoDto, InvestEmployee>().ReverseMap();
            CreateMap<AccountConnectDto, InvestEmployee>().ReverseMap();
        }
    }
}
