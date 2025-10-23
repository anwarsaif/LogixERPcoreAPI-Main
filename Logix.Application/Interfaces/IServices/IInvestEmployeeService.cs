using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.HR.EmployeeDto;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface IInvestEmployeeService : IGenericQueryService<InvestEmployeeDto, InvestEmployeeVvw>, IGenericWriteService<InvestEmployeeDto, InvestEmployeeEditDto>
    {

        Task<IResult<InvestEmployeeDto>> Add(InvestEmployeeAddDto entity, CancellationToken cancellationToken = default);

        Task<IResult<InvestEmployeeDto2>> AddFromMainSys(InvestEmployeeDto2 entity, CancellationToken cancellationToken = default);
        Task<IResult<InvestEmployeeDto2>> UpdateFromMainSys(InvestEmployeeDto2 entity, CancellationToken cancellationToken = default);

        Task<IResult<InvestEmployeeDto>> FastAdd(EmployeeFastAddDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeMainInfoDto>> UpdateMain(EmployeeMainInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeAdditionalPropsDto>> UpdateAdditionalProps(EmployeeAdditionalPropsDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeContactInfoDto>> UpdateContact(EmployeeContactInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeContractInfoDto>> UpdateContract(EmployeeContractInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeDependentsDto>> UpdateDependents(EmployeeDependentsDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeFollowersDto>> UpdateFollowers(EmployeeFollowersDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeJobInfoDto>> UpdateJob(EmployeeJobInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeMedicalInsuranceDto>> UpdateMedicalInsurance(EmployeeMedicalInsuranceDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeePreparingDto>> UpdatePreparing(EmployeePreparingDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeSalaryInfoDto>> UpdateSalary(EmployeeSalaryInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeSocialInsuranceDto>> UpdateSocialInsurance(EmployeeSocialInsuranceDto entity, CancellationToken cancellationToken = default);
        Task<IResult<EmployeeTravelInfoDto>> UpdateTravel(EmployeeTravelInfoDto entity, CancellationToken cancellationToken = default);
        Task<IResult<bool>> ChangeEmployeesStatus(int StatusId, List<string> employeesId, string? Note, CancellationToken cancellationToken = default);
        Task<IResult<bool>> InsuranceUpdate(string Tdate, CancellationToken cancellationToken = default);

        Task<IResult<EmployeeSubDto>> AddEmployeeSub(EmployeeSubDto entity, CancellationToken cancellationToken = default);
        Task<IResult<object>> UpdateEmployeeSub(EmployeeSubDto entity, CancellationToken cancellationToken = default);
        Task<IResult<bool>> ChangeEmployeeManager1(string empId, List<string> employeesId, CancellationToken cancellationToken = default);
        Task<IResult<bool>> ChangeEmployeeManager2(string empId, List<string> employeesId, CancellationToken cancellationToken = default);
        Task<IResult<bool>> ChangeEmployeeManager3(string empId, List<string> employeesId, CancellationToken cancellationToken = default);


        Task<IResult<string>> UpdateContractExpair(List<string> empCodes,string NewDate="", CancellationToken cancellationToken = default);
        Task<IResult<string>> UpdateMedicalInsuranceExpair(List<string> empCodes,string NewDate="", CancellationToken cancellationToken = default);
        Task<IResult<AccountConnectDto>> UpdateConnectAccounts(AccountConnectDto entity, CancellationToken cancellationToken = default);
        Task<IResult<object>> ChangeEmployeeImage(ChangeEmployeeImageDto entity, CancellationToken cancellationToken = default);

        Task<IResult<string>> UpdateIDExpair(HREmpIDExpireUpdateDto obj, CancellationToken cancellationToken = default);
        Task<string> GetTimeZone(int Id, CancellationToken cancellationToken = default);
        Task<long> GetManagerId(string managerEmpId, CancellationToken cancellationToken = default);
        Task<IResult<SelectList>> DDLFieldColumns();
    }
}
