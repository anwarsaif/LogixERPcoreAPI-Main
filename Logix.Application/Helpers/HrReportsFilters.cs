using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Services;
using Logix.Domain.HR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Helpers
{
  public class HrReportsFilters
  {
    private readonly IHrRepositoryManager hrRepositoryManager;
    private readonly IMainRepositoryManager mainRepositoryManager;
    private readonly ILocalizationService localization;
    private readonly IMapper _mapper;
    private readonly ICurrentData session;

    public HrReportsFilters(
        IHrRepositoryManager hrRepositoryManager,
        IMainRepositoryManager mainRepositoryManager,
        IMapper mapper,
        ICurrentData session,
        ILocalizationService localization)
    {
      this.hrRepositoryManager = hrRepositoryManager;
      this.mainRepositoryManager = mainRepositoryManager;
      _mapper = mapper;
      this.session = session;
      this.localization = localization;
    }

    // تقدر تضيف الميثودات اللي تحتاجها
    public static EmployeeFilterDto GetHrEmployee(Dictionary<string, string> dictionary)
    {
      return new EmployeeFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
        //JDateGregorian = dictionary["JDateGregorian"],
        //JDateGregorian2 = dictionary["JDateGregorian2"],
      };
    }
    public static Application.DTOs.Main.HrEmployeeBendingFilterVM GetHrEmployeeBending(Dictionary<string, string> dictionary)
    {
      return new Application.DTOs.Main.HrEmployeeBendingFilterVM()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
        //JDateGregorian = dictionary["JDateGregorian"],
      };
    }
    public static HrLeaveFilterDto GetHrEndOfService(Dictionary<string, string> dictionary)
    {
      return new HrLeaveFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrTransferFilterDto GetHrTransfer(Dictionary<string, string> dictionary)
    {
      string GetValue(string key, string defaultValue = "")
      {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
      }

      return new HrTransferFilterDto()
      {
        EmpId = GetValue("EmpId", ""),
        EmpName = GetValue("EmpName", ""),
        DeptId = Convert.ToInt32(GetValue("DeptId", "0")),
        BranchId = Convert.ToInt32(GetValue("BranchId", "0")),
        BranchToId = Convert.ToInt64(GetValue("BranchToId", "0")),
        LocationId = Convert.ToInt32(GetValue("LocationId", "0")),
        LocationFromId = Convert.ToInt64(GetValue("LocationFromId", "0")),
        LocationToId = Convert.ToInt64(GetValue("LocationToId", "0")),
        TransDepartmentFrom = Convert.ToInt64(GetValue("TransDepartmentFrom", "0")),
        TransDepartmentTo = Convert.ToInt64(GetValue("TransDepartmentTo", "0")),
        FromDate = GetValue("FromDate", ""),
        ToDate = GetValue("ToDate", "")
      };
    }
    public static HrLoanFilterDto GetHrLoan(Dictionary<string, string> dictionary)
    {
      return new HrLoanFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrAllowanceDeductionOtherFilterDto GetHrAllowanceDeduction(Dictionary<string, string> dictionary)
    {
      return new HrAllowanceDeductionOtherFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrIncrementFilterDto GetHrIncrement(Dictionary<string, string> dictionary)
    {
      return new HrIncrementFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRRPEmpStatusHistoryFilterDto GetHrEmpStatus(Dictionary<string, string> dictionary)
    {
      return new HRRPEmpStatusHistoryFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrDirectJobFilterDto GetHrJoinWork(Dictionary<string, string> dictionary)
    {
      string GetValue(string key, string defaultValue = "")
      {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
      }

      return new HrDirectJobFilterDto()
      {
        EmpId = GetValue("EmpId", ""),
        EmpName = GetValue("EmpName", ""),
        DeptId = Convert.ToInt32(GetValue("DeptId", "0")),
        BranchId = Convert.ToInt32(GetValue("BranchId", "0")),
        LocationId = Convert.ToInt32(GetValue("LocationId", "0")),
        From = GetValue("From", ""),
        To = GetValue("To", "")
      };
    }
    public static HrAttendancesFilterDto GetHrHrAttendance(Dictionary<string, string> dictionary)
    {
      return new HrAttendancesFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceReport4FilterDto GetHrHrAttendanceData(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceReport4FilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrCheckInOutFilterDto GetHrActualAttendance(Dictionary<string, string> dictionary)
    {
      return new HrCheckInOutFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceReportFilterDto GetHrAttendanceReport(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceReportFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceTotalReportSPFilterDto GetHrAttendanceTotalReport(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceTotalReportSPFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrAbsenceFilterDto GetHrAbsence(Dictionary<string, string> dictionary)
    {
      return new HrAbsenceFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrOverTimeMFilterDto GetHrOverTimeM(Dictionary<string, string> dictionary)
    {
      return new HrOverTimeMFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrDelayFilterDto GetHrDelay(Dictionary<string, string> dictionary)
    {
      return new HrDelayFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRApprovalAbsencesReportFilterDto GetHrApprovalAbsence(Dictionary<string, string> dictionary)
    {
      return new HRApprovalAbsencesReportFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrPermissionFilterDto GetHrPermission(Dictionary<string, string> dictionary)
    {
      return new HrPermissionFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceReport5FilterDto GetHrAttendanceReportForEmp(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceReport5FilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HrAttendanceUnknownFilterDto GetHrAttendanceUnknown(Dictionary<string, string> dictionary)
    {
      return new HrAttendanceUnknownFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceReport6FilterSP GetHrAttendanceReportDays(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceReport6FilterSP()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceTotalReportFilterDto GetHrAttendanceTotalFromTo(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceTotalReportFilterDto()
      {
        //PeriodId = Convert.ToInt64(dictionary["PeriodId"]),
        //JCode = dictionary["JCode"],
        //JCode2 = dictionary["JCode2"],
      };
    }
    public static HRAttendanceCheckingStaffFilterDto GetHrCheckingStaff(Dictionary<string, string> dictionary)
    {
      return new HRAttendanceCheckingStaffFilterDto()
      {
        empCode = dictionary["empCode"],
        Date = dictionary["Date"],
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrVacationsFilterDto GetHRVacations(Dictionary<string, string> dictionary)
    {
      return new HrVacationsFilterDto()
      {
        EmpCode = dictionary["EmpCode"],
        EmpName = dictionary["EmpName"],
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        ClearnceId = Convert.ToInt32(dictionary["ClearnceId"]),
        StartDate = dictionary["StartDate"],
        EndDate = dictionary["EndDate"],
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
        VacationTypeId = Convert.ToInt32(dictionary["VacationTypeId"]),
        LocationId = Convert.ToInt32(dictionary["LocationId"]),
        TransTypeId = Convert.ToInt32(dictionary["TransTypeId"])
      };
    }
    public static HrVacationBalanceALLSendFilterDto GetHRVacationBalanceALL(Dictionary<string, string> dictionary)
    {
      return new HrVacationBalanceALLSendFilterDto()
      {
        BranchId = Convert.ToInt64(dictionary["BranchId"]),
        FacilityId = Convert.ToInt32(dictionary["FacilityId"]),
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
        JobCatagoriesId = Convert.ToInt32(dictionary["JobCatagoriesId"]),
        Location = Convert.ToInt32(dictionary["Location"]),
        StatusId = Convert.ToInt32(dictionary["StatusId"]),
        NationalityId = Convert.ToInt32(dictionary["NationalityId"]),
        VacationTypeId = Convert.ToInt32(dictionary["VacationTypeId"]),
        EmpId = dictionary["EmpId"].ToString(),
        EmpName = dictionary["EmpName"],
        CurrentDate = dictionary["CurrentDate"]
      };
    }
    public static HrVacationBalanceFilterDto GetHRVacationBalance(Dictionary<string, string> dictionary)
    {
      return new HrVacationBalanceFilterDto()
      {
        //BranchId = Convert.ToInt64(dictionary["BranchId"]),
        //FacilityId = Convert.ToInt32(dictionary["FacilityId"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrVacationEmpBalanceDto GetHRVacationEmpBalance(Dictionary<string, string> dictionary)
    {
      return new HrVacationEmpBalanceDto()
      {
        Emp_Code = dictionary["Emp_Code"],
        Currentdate = dictionary["Currentdate"]
      };
    }
    public static HrVacationsFilterDto GetHRVacationReport(Dictionary<string, string> dictionary)
    {
      return new HrVacationsFilterDto()
      {
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
        EmpCode = dictionary["EmpCode"],
        LocationId = Convert.ToInt32(dictionary["LocationId"]),
        StartDate = dictionary["StartDate"],
        EndDate = dictionary["EndDate"],
        VacationTypeId = Convert.ToInt32(dictionary["VacationTypeId"]),
        ChkGroupByEmpAndVacation = Convert.ToBoolean(dictionary["ChkGroupByEmpAndVacation"])
      };
    }
    public static HrRPVacationEmployeeFilterDto GetHRRPVacationEmployee(Dictionary<string, string> dictionary)
    {
      return new HrRPVacationEmployeeFilterDto()
      {
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        LocationId = Convert.ToInt32(dictionary["LocationId"]),
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
        EmpCode = dictionary["EmpCode"],
        StartDate = dictionary["StartDate"],
        EndDate = dictionary["EndDate"],
        VacationTypeId = Convert.ToInt32(dictionary["VacationTypeId"])
      };
    }
    public static HrOpeningBalanceFilterDto GetHRBalance(Dictionary<string, string> dictionary)
    {
      return new HrOpeningBalanceFilterDto()
      {
        TypeId = Convert.ToInt64(dictionary["TypeId"]),
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        EmpCode = dictionary["EmpCode"]
      };
    }
    public static HrRecruitmentCandidateFilterDto GetHRRecruitmentCandidate(Dictionary<string, string> dictionary)
    {
      return new HrRecruitmentCandidateFilterDto()
      {
        //BranchId = Convert.ToInt64(dictionary["BranchId"]),
        //FacilityId = Convert.ToInt32(dictionary["FacilityId"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HRRepKPIFilterDto GetHrRepKPI(Dictionary<string, string> dictionary)
    {
      return new HRRepKPIFilterDto()
      {
        //BranchId = Convert.ToInt64(dictionary["BranchId"]),
        //FacilityId = Convert.ToInt32(dictionary["FacilityId"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static CurrentBalanceFilterDto GetHrCurrBalance(Dictionary<string, string> dictionary)
    {
      return new CurrentBalanceFilterDto()
      {
        EmpCode = dictionary["EmpCode"],
        TypeId = Convert.ToInt32(dictionary["TypeId"]),
        CurrDate = dictionary["CurrDate"],
      };
    }

    public static CurrentBalanceFilterDto GetHrCurrBalanceAll(Dictionary<string, string> dictionary)
    {
      return new CurrentBalanceFilterDto()
      {
        EmpCode = dictionary["EmpCode"],
        TypeId = Convert.ToInt32(dictionary["TypeId"]),
        CurrDate = dictionary["CurrDate"],
      };
    }

    public static HrPayrollFilterDto GetHrPayrollByBranch(Dictionary<string, string> dictionary)
    {
      return new HrPayrollFilterDto()
      {
        FinancelYear = Convert.ToInt32(dictionary["FinancelYear"]),
        BranchId = Convert.ToInt64(dictionary["BranchId"]),
        //MsMonth = dictionary["MsMonth"],
        PayrollTypeId = Convert.ToInt32(dictionary["PayrollTypeId"]),
        FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
        //MsId = Convert.ToInt64(dictionary["MsId"]),
        //MsCode = Convert.ToInt64(dictionary["MsCode"]),
        //MsDate = dictionary["MsDate"],
        //MsTitle = dictionary["MsTitle"],
        //MsMonthName = dictionary["MsMonthName"],
        //StatusName = dictionary["StatusName"],
        //TypeName = dictionary["TypeName"],
        //TypeName2 = dictionary["TypeName2"],
        //StatusName2 = dictionary["StatusName2"],
        //ApplicationCode = Convert.ToInt64(dictionary["ApplicationCode"]),
        //BranchsId = dictionary["BranchsId"],
        //Status = Convert.ToInt32(dictionary["Status"]),
      };
    }
    public static HrPayrollQueryFilterDto GetHrPayrollQuery(Dictionary<string, string> dictionary)
    {
      return new HrPayrollQueryFilterDto()
      {
        //FinancelYear = Convert.ToInt32(dictionary["FinancelYear"]),
        //BranchId = Convert.ToInt64(dictionary["BranchId"]),
        ////MsMonth = dictionary["MsMonth"],
        //PayrollTypeId = Convert.ToInt32(dictionary["PayrollTypeId"]),
        //FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
        ////MsId = Convert.ToInt64(dictionary["MsId"]),
        ////MsCode = Convert.ToInt64(dictionary["MsCode"]),
        ////MsDate = dictionary["MsDate"],
        ////MsTitle = dictionary["MsTitle"],
        ////MsMonthName = dictionary["MsMonthName"],
        ////StatusName = dictionary["StatusName"],
        ////TypeName = dictionary["TypeName"],
        ////TypeName2 = dictionary["TypeName2"],
        ////StatusName2 = dictionary["StatusName2"],
        ////ApplicationCode = Convert.ToInt64(dictionary["ApplicationCode"]),
        ////BranchsId = dictionary["BranchsId"],
        ////Status = Convert.ToInt32(dictionary["Status"]),
      };
    }

    //public async Task<List<HrPayrollDVw>> GetPayrollCheckB(long payrIds)
    //{
    //  try
    //  {
    //    var payrollResult = await hrRepositoryManager.HrPayrollDRepository.GetOneVw(x => x.MsdId == payrIds && x.IsDeleted == false);

    //    // التحقق من أن النتيجة ناجحة وأن بها بيانات
    //    if (payrollResult != null)
    //    {
    //      return new List<HrPayrollDVw> { payrollResult };
    //    }

    //    return new List<HrPayrollDVw>();
    //  }
    //  catch
    //  {
    //    return new List<HrPayrollDVw>();
    //  }
    //}
    public async Task<List<HrPayrollDVw>> GetPayrollCheckB(long payrIds)
    {
      try
      {
        var payrollResults = await hrRepositoryManager.HrPayrollDRepository
            .GetAllVw(x => x.MsdId == payrIds && x.IsDeleted == false);

        return payrollResults?.ToList() ?? new List<HrPayrollDVw>();
      }
      catch
      {
        return new List<HrPayrollDVw>();
      }
    }


    public static HrUnpaidEmployeesFilter GetHrUnpaidEmployees(Dictionary<string, string> dictionary)
    {
      return new HrUnpaidEmployeesFilter()
      {
        FinancelYear = Convert.ToInt32(dictionary["FinancelYear"]),
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        MsMonth = dictionary["MsMonth"],
        EmpName = dictionary["EmpName"],
        JobCatagoriesId = Convert.ToInt32(dictionary["JobCatagoriesId"]),
        StatusId = Convert.ToInt32(dictionary["StatusId"]),
        NationalityId = Convert.ToInt32(dictionary["NationalityId"]),
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }


    public async Task<List<HrEmployeeVw>> GetEmployeeData(long empId)
    {
      try
      {
        var getEmpData = await hrRepositoryManager.HrEmployeeRepository.GetOneVw(
            x => x.Id == empId && x.IsDeleted == false
        );              // التحقق من أن النتيجة ناجحة وأن بها بيانات
        if (getEmpData != null)
        {
          return new List<HrEmployeeVw> { getEmpData };
        }

        return new List<HrEmployeeVw>();
      }
      catch
      {
        return new List<HrEmployeeVw>();
      }
    }

    public static HrPayrollFilterDto GetHrPayrollByLocation(Dictionary<string, string> dictionary)
    {
      return new HrPayrollFilterDto()
      {
        FinancelYear = Convert.ToInt32(dictionary["FinancelYear"]),
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        PayrollTypeId = Convert.ToInt32(dictionary["PayrollTypeId"]),
        FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
      };
    }
    public static HrPayrollFilterDto GetHrPayrollByDep(Dictionary<string, string> dictionary)
    {
      return new HrPayrollFilterDto()
      {
        FinancelYear = Convert.ToInt32(dictionary["FinancelYear"]),
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        PayrollTypeId = Convert.ToInt32(dictionary["PayrollTypeId"]),
        FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
      };
    }
    public async Task<List<HrDisciplinaryCaseActionVw>> GetDisciplinaryCaseAction(long empId)
    {
      try
      {
        var items = await hrRepositoryManager.HrDisciplinaryCaseActionRepository.GetOneVw(
            x => x.Id == empId && x.IsDeleted == false
        );              // التحقق من أن النتيجة ناجحة وأن بها بيانات
        if (items != null)
        {
          return new List<HrDisciplinaryCaseActionVw> { items };
        }

        return new List<HrDisciplinaryCaseActionVw>();
      }
      catch
      {
        return new List<HrDisciplinaryCaseActionVw>();
      }
    }

    public static HrQualificationsFilterDto GetHRQualifications(Dictionary<string, string> dictionary)
    {
      return new HrQualificationsFilterDto()
      {
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        JobType = Convert.ToInt32(dictionary["JobType"]),
        DeptId = Convert.ToInt32(dictionary["DeptId"]),
        NationalityId = Convert.ToInt32(dictionary["NationalityId"]),
        JobCatagoriesId = Convert.ToInt32(dictionary["JobCatagoriesId"]),
        Status = Convert.ToInt32(dictionary["Status"]),
        EmpCode = dictionary["EmpCode"],
        EmpName = dictionary["EmpName"],
        IdNo = dictionary["IdNo"],
        PassId = dictionary["PassId"],
        EntryNo = dictionary["EntryNo"],
      };
    }

    public async Task<List<HrProvisionsMedicalInsuranceEmployeeVw>> GetProvisionsMedicalInsuranceEmployee(long empId)
    {
      try
      {
        var items = await hrRepositoryManager.HrProvisionsMedicalInsuranceEmployeeRepository.GetOneVw(
          x => x.PId == empId && x.IsDeleted == false
        );              // التحقق من أن النتيجة ناجحة وأن بها بيانات
        if (items != null)
        {
          return new List<HrProvisionsMedicalInsuranceEmployeeVw> { items };
        }

        return new List<HrProvisionsMedicalInsuranceEmployeeVw>();
      }
      catch
      {
        return new List<HrProvisionsMedicalInsuranceEmployeeVw>();
      }
    }

    public async Task<List<HrProvisionsEmployeeVw>> GetProvisionsEmployee(long empId)
    {
      try
      {
        var items = await hrRepositoryManager.HrProvisionsEmployeeRepository.GetOneVw(
          x => x.PId == empId && x.IsDeleted == false
        );              // التحقق من أن النتيجة ناجحة وأن بها بيانات
        if (items != null)
        {
          return new List<HrProvisionsEmployeeVw> { items };
        }

        return new List<HrProvisionsEmployeeVw>();
      }
      catch
      {
        return new List<HrProvisionsEmployeeVw>();
      }
    }

    public static HRRPOhadFilterDto GetHRReportOhad(Dictionary<string, string> dictionary)
    {
      return new HRRPOhadFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrEmployeeCostFilterDto GetHrEmployeeCost(Dictionary<string, string> dictionary)
    {
      return new HrEmployeeCostFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrEmployeeFileFilterDto GetHrEmployeeFile(Dictionary<string, string> dictionary)
    {
      return new HrEmployeeFileFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HREmpIDExpireReportFilterDto GetHREmpIDExpireReport(Dictionary<string, string> dictionary)
    {
      return new HREmpIDExpireReportFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static RPPassportFilterDto GetHRRPPassport(Dictionary<string, string> dictionary)
    {
      return new RPPassportFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrRPContractFilterDto GetRPContract(Dictionary<string, string> dictionary)
    {
      return new HrRPContractFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static RPMedicalInsuranceFilterDto GetRPMedicalInsurance(Dictionary<string, string> dictionary)
    {
      return new RPMedicalInsuranceFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static DOAppointmentFilterDto GetDOAppointment(Dictionary<string, string> dictionary)
    {
      return new DOAppointmentFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static RPAttendFilterDto GetRPAttend(Dictionary<string, string> dictionary)
    {
      return new RPAttendFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static RPBankFilterDto GetRPBank(Dictionary<string, string> dictionary)
    {
      return new RPBankFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static HrStaffSalariesAllowancesDeductionsFilterDto GetHrStaffSalariesAllowancesDeductions(Dictionary<string, string> dictionary)
    {
      return new HrStaffSalariesAllowancesDeductionsFilterDto()
      {
        //BranchId = Convert.ToInt32(dictionary["BranchId"]),
        //JobType = Convert.ToInt32(dictionary["JobType"]),
        //DeptId = Convert.ToInt32(dictionary["DeptId"]),
      };
    }
    public static AccountBalanceSheetFilterDto GetHRAccountTransactions(Dictionary<string, string> dictionary)
    {
      return new AccountBalanceSheetFilterDto()
      {
        FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
        FinancialYear = Convert.ToInt32(dictionary["FinancialYear"]),
        BranchId = Convert.ToInt32(dictionary["BranchId"]),
        referenceTypeId = Convert.ToInt32(dictionary["referenceTypeId"]),
        EmpCode = dictionary["EmpCode"],
        FromDate = dictionary["FromDate"],
        ToDate = dictionary["ToDate"],
        chkAllYear = Convert.ToBoolean(dictionary["chkAllYear"]),
      };
    }
  }
 }
