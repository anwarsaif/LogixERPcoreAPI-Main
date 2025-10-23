using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IResult = Logix.Application.Wrapper.IResult;

using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using Logix.Application.Interfaces.IServices;
using System.Reflection.Emit;

namespace Logix.Application.Services.Main
{
    public class SysTemplateService : GenericQueryService<SysTemplate, SysTemplateDto, SysTemplateVw>, ISysTemplateService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHrRepositoryManager hrRepositoryManager;


        public SysTemplateService(IQueryRepository<SysTemplate> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization, IWebHostEnvironment _hostingEnvironment, IHrRepositoryManager hrRepositoryManager) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
            this._hostingEnvironment = _hostingEnvironment;
            this.hrRepositoryManager = hrRepositoryManager;
        }

        public async Task<IResult<SysTemplateDto>> Add(SysTemplateDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysTemplateDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysTemplate>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysTemplateRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysTemplateDto>(newEntity);

                return await Result<SysTemplateDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysTemplateDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }



        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysTemplateRepository.GetById(Id);
                if (item == null) return Result<SysTemplateDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysTemplateRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysTemplateDto>.SuccessAsync(_mapper.Map<SysTemplateDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysTemplateDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysTemplateRepository.GetById(Id);
                if (item == null) return Result<SysTemplateDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysTemplateRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysTemplateDto>.SuccessAsync(_mapper.Map<SysTemplateDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysTemplateDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysTemplateEditDto>> Update(SysTemplateEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysTemplateEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysTemplateRepository.GetById(entity.Id ?? 0);

                if (item == null) return await Result<SysTemplateEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                if (entity.TypeId == 1)
                    entity.Url = null;

                if (entity.TypeId == 2)
                    entity.Detailes = null;

                _mapper.Map(entity, item);
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysTemplateRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysTemplateEditDto>.SuccessAsync(_mapper.Map<SysTemplateEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysTemplateEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<string> ReplaceDate(string document, int typeId, long empId, int TemplateId)
        {
            string details = "";
            string[] values;
            string[] sValues;

            // Fetch employee template details using the new method
            var employee = await GetPrintTemplateData(empId);

            if (employee == null) return details;

            // Create a list of field names (these are similar to Valus in VB.NET code)
            var fieldNames = new List<string>
    {
        "Location_Name2", "Location_Name", "Dep_Name", "Nationality_Name2", "Nationality_Name",
        "Cat_Name2", "Cat_Name", "Emp_Name2", "Emp_Name", "ID_NO", "ID_Expire_Date", "Contract_expiry_Date",
        "Contract_Date", "Home_Phone", "Mobile", "POBox", "Postal_Code", "Email", "Salary", "DOAppointment",
        "Curr_Date", "End_Of_Service", "Net", "Total", "Total_Write", "Housing_allowance", "Transport_allowance",
        "Nature_allowance", "Foods_allowance", "Risk_allowance", "Costofliving_allowance", "Fuels_allowance",
        "Endofservice_allowance", "Vaction_allowance", "Other_allowance1", "Others_allowance", "Shift_Name", "Emp_ID"
    };

            // Create a corresponding list of values from the employee data
            var fieldValues = new List<string>
    {
        employee.LocationName2 ?? "", employee.LocationName ?? "", employee.DepName ?? "", employee.NationalityName2 ?? "", employee.NationalityName ?? "",
        employee.CatName2 ?? "", employee.CatName ?? "", employee.EmpName2 ?? "", employee.EmpName ?? "", employee.IdNo ?? "", employee.IdExpireDate ?? "",
        employee.ContractExpiryDate ?? "", employee.ContractDate ?? "", employee.HomePhone ?? "",
        employee.Mobile ?? "", employee.POBox ?? "", employee.PostalCode ?? "", employee.Email ?? "", employee.Salary?.ToString() ?? "",
        employee.DOAppointment ?? "", employee.CurrDate ?? "", employee.EndOfService?.ToString() ?? "", employee.Net?.ToString() ?? "",
        employee.Total?.ToString() ?? "", employee.TotalWrite ?? "", employee.HousingAllowance?.ToString() ?? "",
        employee.TransportAllowance?.ToString() ?? "", employee.NatureAllowance?.ToString() ?? "", employee.FoodsAllowance?.ToString() ?? "",
        employee.RiskAllowance?.ToString() ?? "", employee.CostOfLivingAllowance?.ToString() ?? "", employee.FuelsAllowance?.ToString() ?? "",
        employee.EndOfServiceAllowance?.ToString() ?? "", employee.VacationAllowance?.ToString() ?? "", employee.OtherAllowance1?.ToString() ?? "",
        employee.OthersAllowance?.ToString() ?? "", employee.ShiftName ?? "", employee.EmpCode ?? ""
    };

            values = fieldNames.ToArray();
            sValues = fieldValues.ToArray();

            if (typeId == 1)
            {
                var sysTemplate = await _mainRepositoryManager.SysTemplateRepository.GetOne(x => x.IsDeleted == false && x.Id == TemplateId);

                if (sysTemplate != null)
                {
                    details = sysTemplate.Detailes ?? "";
                }

                // Replace placeholders in the details
                for (int i = 0; i < sValues.Length; i++)
                {
                    if (!string.IsNullOrEmpty(values[i]) && !string.IsNullOrEmpty(sValues[i]))
                    {
                        details = details.Replace(values[i], sValues[i]);
                    }
                }
            }
            else if (typeId == 2)
            {
                // Generate new file path to save the updated document
                string newFileName = $"UpdatedDocument_{empId}_{DateTime.Now:yyyyMMddHHmmss}.docx";
                string newFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "Templates", newFileName);
                string fileUrl = $"/Templates/{newFileName}"; // URL relative to the web application root

                var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Templates");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Copy the original document to the new file
                File.Copy(document, newFilePath, true);

                // Modify the copied document
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newFilePath, true))
                {
                    string docText;

                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    // Replace placeholders in the document text
                    for (int i = 0; i < sValues.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(values[i]) && !string.IsNullOrEmpty(sValues[i]))
                        {
                            Regex regexText = new Regex(values[i]);
                            docText = regexText.Replace(docText, sValues[i]);
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                }

                // Return the public file URL to the controller
                return fileUrl;
            }


            return details;
            
        }

        public async Task<PrintTemplateDto?> GetPrintTemplateData(long empId)
        {
            var currentDate = DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            var getEmployee = await hrRepositoryManager.HrEmployeeRepository.GetOneVw(x => x.Id == empId && x.Isdel == false && x.IsDeleted == false);
            if (getEmployee == null)
                return null;
            // Fetch the employee's main details
            var employee = new PrintTemplateDto
            {
                LocationName2 = getEmployee.LocationName2,
                LocationName = getEmployee.LocationName,
                DepName = getEmployee.DepName,
                NationalityName2 = getEmployee.NationalityName2,
                NationalityName = getEmployee.NationalityName,
                CatName2 = getEmployee.CatName2,
                CatName = getEmployee.CatName,
                EmpName2 = getEmployee.EmpName2,
                EmpName = getEmployee.EmpName,
                IdNo = getEmployee.IdNo,
                IdExpireDate = getEmployee.IdExpireDate,
                EmpId = getEmployee.Id,
                ContractExpiryDate = getEmployee.ContractExpiryDate,
                ContractDate = getEmployee.ContractData,
                HomePhone = getEmployee.HomePhone,
                Mobile = getEmployee.Mobile,
                POBox = getEmployee.Pobox,
                PostalCode = getEmployee.PostalCode,
                Email = getEmployee.Email,
                Salary = getEmployee.Salary,
                DOAppointment = getEmployee.Doappointment,
                CurrDate = currentDate,
                EmpCode = getEmployee.EmpId,
                EndOfService = await hrRepositoryManager.HrLeaveRepository.HR_End_Service_Due(currentDate, empId, 1)
            };


            if (employee == null) return null;
            decimal? totalAllowanceAmount = 0;
            decimal? totalDeductionAmount = 0;
            // Calculate Allowances and Deductions
            var totalAllowanceDeductions = await hrRepositoryManager.HrAllowanceDeductionRepository
                .GetAll(ad => ad.EmpId == empId && ad.IsDeleted == false && ad.FixedOrTemporary == 1);

            totalAllowanceAmount = totalAllowanceDeductions.Where(x => x.TypeId == 1).Sum(x => x.Amount);
            totalDeductionAmount = totalAllowanceDeductions.Where(x => x.TypeId == 2).Sum(x => x.Amount);

            // Calculate net salary
            employee.Net = employee.Salary + totalAllowanceAmount - totalDeductionAmount;
            employee.Total = employee.Salary + totalAllowanceAmount;
            employee.TotalWrite = MethodsHelper.Tafkeet(employee.Total ?? 0);

            // Fetch allowances for each category
            employee.HousingAllowance = await GetAllowance(empId, 1);
            employee.TransportAllowance = await GetAllowance(empId, 2);
            employee.NatureAllowance = await GetAllowance(empId, 21);
            employee.FoodsAllowance = await GetAllowance(empId, 3);
            employee.RiskAllowance = await GetAllowance(empId, 20);
            employee.CostOfLivingAllowance = await GetAllowance(empId, 19);
            employee.FuelsAllowance = await GetAllowance(empId, 18);
            employee.EndOfServiceAllowance = await GetAllowance(empId, 16);
            employee.VacationAllowance = await GetAllowance(empId, 15);
            employee.OtherAllowance1 = await GetOtherAllowances(empId, new int[] { 1, 2 });
            employee.OthersAllowance = await GetAllowance(empId, 6);


            employee.ShiftName = await hrRepositoryManager.HrAttShiftRepository.GetShiftNameAsync(empId);

            return employee;
        }

        private async Task<decimal> GetAllowance(long empId, int adId)
        {
            var getData = await hrRepositoryManager.HrAllowanceDeductionRepository
                .GetAll(ad => ad.EmpId == empId && ad.IsDeleted == false && ad.TypeId == 1 && ad.FixedOrTemporary == 1 && ad.AdId == adId);
            var result = getData.Sum(x => x.Amount) ?? 0;
            return result;


        }

        private async Task<decimal> GetOtherAllowances(long empId, int[] excludedIds)
        {

            var getData = await hrRepositoryManager.HrAllowanceDeductionRepository.GetAll(ad =>
                ad.EmpId == empId &&
                !ad.IsDeleted &&
                ad.TypeId == 1 &&
                ad.FixedOrTemporary == 1
            );
            var result = getData.Where(ad => !excludedIds.Contains((int)ad.AdId)).Sum(x => x.Amount) ?? 0;
            return result;
        }

    }
}
