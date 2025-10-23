using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.PUR;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Domain.PUR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Globalization;

namespace Logix.Application.Services.Main
{
    public class SysCustomerService : GenericQueryService<SysCustomer, SysCustomerDto, SysCustomerVw>, ISysCustomerService
    {
        private readonly IAccRepositoryManager accRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly ILocalizationService localization;
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly IWebHostEnvironment env;

        public SysCustomerService(IQueryRepository<SysCustomer> queryRepository, IAccRepositoryManager accRepositoryManager, IHrRepositoryManager hrRepositoryManager, ILocalizationService localization, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, IWebHostEnvironment env) : base(queryRepository, mapper)
        {
            this.accRepositoryManager = accRepositoryManager;
            this.hrRepositoryManager = hrRepositoryManager;
            this.localization = localization;
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.env = env;
        }
        public async Task<IResult<SysCustomerAddVM>> Add(SysCustomerAddVM entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerAddVM>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {

                var item = _mapper.Map<SysCustomer>(entity.SysCustomerDto);
                var days = "";
                if (entity.AttDays != null)
                {
                    foreach (var day in entity.AttDays)
                    {
                        if (day != null && day.Selected)
                        {
                            if (!string.IsNullOrEmpty(days))
                            {
                                days += $",{day.DayNo}";
                            }
                            else
                            {
                                days += $"{day.DayNo}";
                            }

                        }
                    }
                }
                item.DaysOfVisit = days;
                // ================= validations befor add customer ====================
                long accAccountId = 0;
                var salesAccountType = await accRepositoryManager.AccFacilityRepository.GetOne(s => s.SalesAccountType, s => s.FacilityId == session.FacilityId);
                if (salesAccountType != null)
                {
                    if (salesAccountType != 2)
                    {
                        if (!string.IsNullOrEmpty(entity.SysCustomerDto.AccAccountCode))
                        {
                            var accId = await accRepositoryManager.AccAccountRepository.GetOne(s => s.AccAccountId, s => s.FacilityId == session.FacilityId && s.AccAccountCode == entity.SysCustomerDto.AccAccountCode);
                            if (accId != 0)
                            {
                                accAccountId = accId;
                            }
                        }
                    }
                    else
                    {
                        var accBranch = await accRepositoryManager.AccBranchAccountRepository.GetOneVW(s => s.IsDeleted == false && s.BranchId == (long)(item.BranchId ?? 0) && s.Name2 == BranchAccountType.Customer.ToString());
                        if (accBranch != null)
                        {
                            if (accBranch.AccountId != null)
                            {
                                accAccountId = accBranch.AccountId ?? 0;
                            }
                        }
                    }
                }

                item.AccAccountId = accAccountId;

                item.FacilityId = session.FacilityId;

                //------ check IdNo ----------------
                if (!string.IsNullOrEmpty(item.IdNo))
                {
                    var chkIdNo = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.IdNo == item.IdNo && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                    if (chkIdNo != null && chkIdNo.Count() > 0)
                    {
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("IDNumberAlreadyExists"));
                    }
                }

                //------ check mobile ----------------
                if (!string.IsNullOrEmpty(item.Mobile))
                {
                    var chkMobile = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Mobile == item.Mobile && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                    if (chkMobile != null && chkMobile.Count() > 0)
                    {
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("MobileAlreadyExists"));
                    }
                }

                //------ check VatNumber ----------------
                if (!string.IsNullOrEmpty(item.VatNumber))
                {
                    var chkVatNumber = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.VatNumber == item.VatNumber && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                    if (chkVatNumber != null && chkVatNumber.Count() > 0)
                    {
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("الرقم الضريبي موجود سابقاً"));
                    }
                }

                //------ check EmpId ----------------
                if (!string.IsNullOrEmpty(entity.SysCustomerDto.EmpCode))
                {
                    var chkEmp = await hrRepositoryManager.HrEmployeeRepository.GetOne(e => e.EmpId == entity.SysCustomerDto.EmpCode && e.IsDeleted == false);
                    if (chkEmp == null)
                    {
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("SalesPersonDoesNotExistsInEmps"));
                    }
                    else
                    {
                        item.EmpId = chkEmp.Id;
                    }
                }

                //------- chk code ----------------
                var codeReadOnly = await _mainRepositoryManager.SysPropertyValueRepository.GetOne(s => s.PropertyValue, s => s.PropertyId == 179 && s.FacilityId == session.FacilityId);
                if (!string.IsNullOrEmpty(codeReadOnly) && codeReadOnly == "1")
                {
                    if (string.IsNullOrEmpty(item.Code))
                    {
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("SEnterCustomercodeFirst"));
                    }
                    else
                    {
                        var codeExists = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Code == item.Code && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                        if (codeExists != null && codeExists.Count() > 0)
                        {
                            return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("رقم العميل موجود مسبقاً"));
                        }
                    }
                }
                else // create auto code
                {
                    var numberingByBranch = await _mainRepositoryManager.SysPropertyValueRepository.GetOne(s => s.PropertyValue, s => s.PropertyId == 221 && s.FacilityId == session.FacilityId);
                    if (!string.IsNullOrEmpty(numberingByBranch) && numberingByBranch == "1")
                    {
                        var count = _mainRepositoryManager.SysCustomerRepository.Entities.Count(s => s.CusTypeId == item.CusTypeId && s.FacilityId == item.FacilityId && s.BranchId == item.BranchId);
                        var branchCode = await _mainRepositoryManager.InvestBranchRepository.GetOne(s => s.BranchCode, s => s.BranchId == item.BranchId);
                        var code = count + 1;
                        string finalCode = $"{branchCode}{"0000000" + code.ToString().PadLeft(7, '0')}";
                        item.Code = finalCode;
                    }
                    else
                    {
                        var count = _mainRepositoryManager.SysCustomerRepository.Entities.Count(s => s.CusTypeId == item.CusTypeId && s.FacilityId == item.FacilityId);
                        var code = count + 1;
                        string finalCode = $"{"0000000" + code.ToString().PadLeft(7, '0')}";
                        item.Code = finalCode;
                    }
                }

                // -------------------- chk memberId ----------
                if (string.IsNullOrEmpty(item.MemberId))
                {
                    var branchCode = await _mainRepositoryManager.InvestBranchRepository.GetOne(s => s.BranchCode, s => s.BranchId == item.BranchId);
                    var memberId = $"{branchCode}{item.Code}";
                    item.MemberId = memberId;
                }

                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
                var newEntity = await _mainRepositoryManager.SysCustomerRepository.AddAndReturn(item);
                var addRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                if (addRes > 0)
                {
                    if (entity.SysCustomerContactDto != null)
                    {
                        if (!string.IsNullOrEmpty(entity.SysCustomerContactDto.Mobile))
                        {
                            var chkMobile = await _mainRepositoryManager.SysCustomerContactRepository.GetAll(s => s.Mobile == entity.SysCustomerContactDto.Mobile && s.IsDeleted != true, s => s.Id);
                            if (chkMobile != null && chkMobile.Count() > 0)
                            {
                                return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("MobileAlreadyExists"));
                            }
                        }
                        if (!string.IsNullOrEmpty(entity.SysCustomerContactDto.Phone))
                        {
                            var chkMobile = await _mainRepositoryManager.SysCustomerContactRepository.GetAll(s => s.Phone == entity.SysCustomerContactDto.Phone && s.IsDeleted != true, s => s.Id);
                            if (chkMobile != null && chkMobile.Count() > 0)
                            {
                                return await Result<SysCustomerAddVM>.FailAsync($"{localization.GetCommonResource("phone")} already exist");
                            }
                        }
                        var contact = _mapper.Map<SysCustomerContact>(entity.SysCustomerContactDto);

                        contact.IsDeleted = false;
                        contact.CreatedOn = DateTime.Now;
                        contact.CreatedBy = session.UserId;
                        contact.CusId = (int)newEntity.Id;
                        var newContact = await _mainRepositoryManager.SysCustomerContactRepository.AddAndReturn(contact);
                        var addContactRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }

                    if (entity.SysCustomerFiles != null)
                    {
                        var files = _mapper.Map<IEnumerable<SysCustomerFile>>(entity.SysCustomerFiles);
                        foreach (var file in files)
                        {
                            file.CustomerId = newEntity.Id;
                            // var newFile = await _mainRepositoryManager.SysCustomerFileRepository.AddAndReturn(file);
                            var newFile = await SaveCustomerFile(file);
                        }
                        var addfilesRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }

                    await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                }


                return await Result<SysCustomerAddVM>.SuccessAsync(entity, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerAddVM>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        private async Task<SysCustomerFile> SaveCustomerFile(SysCustomerFile file)
        {
            // try
            // {
            string fname = "";
            string unqName = "";
            if (file != null)
            {
                string attFolder = Path.DirectorySeparatorChar + FilesPath.AllFiles;
                unqName = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + file.FileExt;
                fname = Path.Join(attFolder, unqName);

                string permFilePath = Path.Join(env.WebRootPath, fname);
                System.IO.File.Copy(file.FileUrl ?? "", permFilePath, true);
                if (System.IO.File.Exists(file.FileUrl))
                {
                    System.IO.File.Delete(file.FileUrl);
                }
                file.FileUrl = fname;

            }
            var newFile = await _mainRepositoryManager.SysCustomerFileRepository.AddAndReturn(file);
            return newFile;
            //}
            /*            catch (Exception ex)
                        {
                            return default;
                        }*/
        }
        public async Task<IResult<SysCustomerDto>> Add(SysCustomerDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {

                var item = _mapper.Map<SysCustomer>(entity);
                var newEntity = await _mainRepositoryManager.SysCustomerRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerDto>(newEntity);


                return await Result<SysCustomerDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysCustomerEditDto>> Update(SysCustomerEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysCustomerRepository.GetById(entity.Id ?? 0);

            if (item == null) return await Result<SysCustomerEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

            var days = "";
            if (entity.AttDays != null)
            {
                foreach (var day in entity.AttDays)
                {
                    if (day != null && day.Selected)
                    {
                        if (!string.IsNullOrEmpty(days))
                        {
                            days += $",{day.DayNo}";
                        }
                        else
                        {
                            days += $"{day.DayNo}";
                        }

                    }
                }
            }

            _mapper.Map(entity, item);

            item.DaysOfVisit = days;
            //item.CusTypeId = 2;
            item.FacilityId = session.FacilityId;
            item.IsDeleted = false;

            long accAccountId = 0;
            var salesAccountType = await accRepositoryManager.AccFacilityRepository.GetOne(s => s.SalesAccountType, s => s.FacilityId == session.FacilityId);
            if (salesAccountType != null)
            {
                if (salesAccountType != 2)
                {
                    if (!string.IsNullOrEmpty(entity.AccAccountCode))
                    {
                        var accId = await accRepositoryManager.AccAccountRepository.GetOne(s => s.AccAccountId, s => s.FacilityId == session.FacilityId && s.AccAccountCode == entity.AccAccountCode);
                        if (accId != 0)
                        {
                            accAccountId = accId;
                        }
                    }
                }
                else
                {
                    var accBranch = await accRepositoryManager.AccBranchAccountRepository.GetOneVW(s => s.IsDeleted == false && s.BranchId == (long)(item.BranchId ?? 0) && s.Name2 == BranchAccountType.Customer.ToString());
                    if (accBranch != null)
                    {
                        if (accBranch.AccountId != null)
                        {
                            accAccountId = accBranch.AccountId ?? 0;
                        }
                    }
                }
            }

            item.AccAccountId = accAccountId;
            //------ check IdNo ----------------
            if (!string.IsNullOrEmpty(item.IdNo))
            {
                var chkIdNo = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Id != item.Id && s.IdNo == item.IdNo && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                if (chkIdNo != null && chkIdNo.Count() > 0)
                {
                    return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("IDNumberAlreadyExists"));
                }
            }

            //------ check mobile ----------------
            if (!string.IsNullOrEmpty(item.CustomerMobile))
            {
                var chkMobile = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Id != item.Id && s.CustomerMobile == item.CustomerMobile && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                if (chkMobile != null && chkMobile.Count() > 0)
                {
                    return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("MobileAlreadyExists"));
                }
            }

            //------ check VatNumber ----------------
            if (!string.IsNullOrEmpty(item.VatNumber))
            {
                var chkVatNumber = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Id != item.Id && s.VatNumber == item.VatNumber && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                if (chkVatNumber != null && chkVatNumber.Count() > 0)
                {
                    return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("الرقم الضريبي موجود سابقاً"));
                }
            }

            //------ check EmpId ----------------
            if (!string.IsNullOrEmpty(entity.EmpCode))
            {
                var chkEmp = await hrRepositoryManager.HrEmployeeRepository.GetOne(e => e.EmpId == entity.EmpCode && e.IsDeleted == false);
                if (chkEmp == null)
                {
                    return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("SalesPersonDoesNotExistsInEmps"));
                }
                else
                {
                    item.EmpId = chkEmp.Id;
                }
            }

            _mainRepositoryManager.SysCustomerRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerEditDto>.SuccessAsync(_mapper.Map<SysCustomerEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysCustomerEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerRepository.GetById(Id);
                if (item == null) return Result<SysCustomerDto>.Fail($"--- there is no Data with this id: {Id}---");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysCustomerRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerDto>.SuccessAsync(_mapper.Map<SysCustomerDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerRepository.GetById(Id);
                if (item == null) return Result<SysCustomerDto>.Fail($"--- there is no Data with this id: {Id}---");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysCustomerRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerDto>.SuccessAsync(_mapper.Map<SysCustomerDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysCustomerAddQVDto>> AddQualifiedVendor(SysCustomerAddQVDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerAddQVDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var checkIdNo = await _mainRepositoryManager.SysCustomerRepository.AnyAsync(
                c => c.IdNo == entity.IdNo &&
                 c.FacilityId == session.FacilityId &&
                 c.CusTypeId == entity.CusTypeId &&
                 c.IsDeleted == false);

                if (checkIdNo)
                    return await Result<SysCustomerAddQVDto>.FailAsync(localization.GetResource1("IdentityExist"));

                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var item = _mapper.Map<SysCustomer>(entity);

                item.IdNo = entity.IdNo;
                item.IdDate = entity.IdDate;
                item.IdIssuer = entity.IdIssuer;
                item.IdType = entity.IdType;
                item.JobAddress = entity.JobAddress;
                item.JobName = entity.JobName;
                item.Mobile = entity.Mobile;
                item.Name = entity.Name;
                item.NationalityId = entity.NationalityId;
                item.Note = entity.Note;
                item.Phone = entity.Phone;
                item.Fax = entity.Fax;
                item.Photo = "";
                item.RepresentedBy = "";

                item.SponsorId = entity.SponsorId;
                item.SponsorJobAddress = entity.SponsorJobAddress;
                item.SponsorJobName = entity.SponsorJobName;
                item.SponsorMobile = entity.SponsorMobile;
                item.SponsorName = entity.SponsorName;
                item.SponsorPhone = entity.SponsorPhone;

                item.Email = entity.Email;
                item.BankAccount = entity.BankAccount;
                item.BankId = entity.BankId;
                item.CityId = 0;
                item.BranchId = entity.BranchId;
                item.CusTypeId = 8;
                item.Address = entity.Address;
                item.AccSeparate = false;

                item.FacilityId = session.FacilityId;
                item.GroupId = entity.GroupId;
                item.CurrencyId = entity.CurrencyId;
                item.AccAccountId = 0;
                item.VatEnable = entity.VatEnable;
                item.VatNumber = entity.VatNumber;
                item.Name2 = entity.Name2;
                item.CustomerName = entity.CustomerName;

                var codeMemberIdDto = await _mainRepositoryManager.SysCustomerRepository.
                    GetCustomerMemberIdCodeAsync(item.CusTypeId ?? 0, item.FacilityId ?? 0, item.BranchId ?? 0,
                    !string.IsNullOrEmpty(item.Code) ? item.Code : string.Empty,
                    !string.IsNullOrEmpty(item.MemberId) ? item.MemberId : string.Empty);

                item.Code = codeMemberIdDto.Code;
                item.MemberId = codeMemberIdDto.MemberId;

                var newEntity = await _mainRepositoryManager.SysCustomerRepository.AddAndReturn(item);
                var addRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                if (addRes > 0)
                {
                    if (entity.FileDtos != null)
                    {
                        var files = _mapper.Map<IEnumerable<SysCustomerFile>>(entity.FileDtos);
                        foreach (var file in files)
                        {
                            file.CustomerId = newEntity.Id;
                            var newFile = await SaveCustomerFile(file);
                        }
                        var addfilesRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }

                    await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                }
                return await Result<SysCustomerAddQVDto>.SuccessAsync(entity, localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerAddQVDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }        
        public async Task<IResult<SysCustomerAddPVDto>> AddPortalVendor(SysCustomerAddPVDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerAddPVDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var checkIdNo = await _mainRepositoryManager.SysCustomerRepository.AnyAsync(
                c => c.IdNo == entity.IdNo &&
                 c.FacilityId == session.FacilityId &&
                 c.CusTypeId == entity.CusTypeId &&
                 c.IsDeleted == false);

                if (checkIdNo)
                    return await Result<SysCustomerAddPVDto>.FailAsync(localization.GetResource1("IdentityExist"));

                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var item = _mapper.Map<SysCustomer>(entity);

                item.Photo = "";
                item.RepresentedBy = "";
                item.BranchId = 1;
                item.FacilityId = 1;
                item.CusTypeId = 8;
                item.CityId = 0;

                var codeMemberIdDto = await _mainRepositoryManager.SysCustomerRepository.
                    GetCustomerMemberIdCodeAsync(item.CusTypeId ?? 0, item.FacilityId ?? 0, item.BranchId ?? 0,
                    !string.IsNullOrEmpty(item.Code) ? item.Code : string.Empty,
                    !string.IsNullOrEmpty(item.MemberId) ? item.MemberId : string.Empty);

                item.Code = codeMemberIdDto.Code;
                item.MemberId = codeMemberIdDto.MemberId;

                var newEntity = await _mainRepositoryManager.SysCustomerRepository.AddAndReturn(item);
                var addRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                if (addRes > 0)
                {
                    if (entity.FileTypesDto != null)
                    {
                        var files = _mapper.Map<IEnumerable<SysCustomerFile>>(entity.FileTypesDto);
                        foreach (var file in files)
                        {
                            file.CustomerId = newEntity.Id;
                            var newFile = await SaveCustomerFile(file);
                        }
                        var addfilesRes = await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }

                    await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                }
                return await Result<SysCustomerAddPVDto>.SuccessAsync(entity, localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerAddPVDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
        public async Task<IResult<SysCustomerEditQVDto>> UpdateQualifiedVendor(SysCustomerEditQVDto entity, bool approve, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));

            var customer = await _mainRepositoryManager.SysCustomerRepository.GetById(entity.Id);
            if (customer == null)
                return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetResource1("NoData"));
            try
            {
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var checkIdNo = await _mainRepositoryManager.SysCustomerRepository.AnyAsync(
                    c => c.Id != entity.Id && c.IdNo == entity.IdNo && c.FacilityId == session.FacilityId &&
                         c.CusTypeId == entity.CusTypeId && c.IsDeleted == false);

                if (checkIdNo)
                    return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetResource1("IdentityExist"));

                _mapper.Map(entity, customer);
                customer.FacilityId = session.FacilityId;
                customer.Photo = "";
                customer.RepresentedBy = "";
                customer.CityId = 0;
                customer.CusTypeId = 8;
                customer.AccAccountId = 0;
                
                if(approve == true)
                {
                    customer.Fax = entity.Fax;
                    customer.Photo = ""; // Default reset
                    customer.RepresentedBy = ""; // Default reset
                    customer.Email = entity.Email;
                    customer.BankAccount = entity.BankAccount;
                    customer.BankId = entity.BankId;
                    customer.CityId = 0; // Default reset
                    customer.BranchId = entity.BranchId;
                    customer.ModifiedBy = entity.ModifiedBy;
                    customer.CusTypeId = 1; // Vendor type
                    customer.Address = entity.Address;
                    customer.AccAccountId = entity.AccAccountId;
                    customer.GroupId = entity.GroupId;
                    customer.CurrencyId = entity.CurrencyId;
                    customer.VatEnable = entity.VatEnable;
                    customer.VatNumber = entity.VatNumber;
                    customer.Name2 = entity.Name2;
                    customer.Enable = entity.Enable;
                    customer.CustomerName = entity.CustomerName;
                    customer.FacilityId = session.FacilityId;

                    customer.SponsorId = entity.SponsorId;
                    customer.SponsorJobAddress = entity.SponsorJobAddress;
                    customer.SponsorJobName = entity.SponsorJobName;
                    customer.SponsorMobile = entity.SponsorMobile;
                    customer.SponsorName = entity.SponsorName;
                    customer.SponsorPhone = entity.SponsorPhone;
                    customer.SponsorEmail = entity.SponsorEmail;
                }

                _mainRepositoryManager.SysCustomerRepository.Update(customer);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerEditQVDto>(customer);

                if (entity.FileDtos != null)
                {
                    var files = _mapper.Map<IEnumerable<SysCustomerFile>>(entity.FileDtos);

                    foreach (var file in files)
                    {
                        if (file.Id == 0)
                        {
                            file.CustomerId = customer.Id;
                            var newFile = await SaveCustomerFile(file);
                        }
                        else
                        {
                            file.CustomerId = customer.Id;
                            var oldFile = await _mainRepositoryManager.SysCustomerFileRepository.GetById(file.Id);

                            if (oldFile == null) continue;
                            _mapper.Map(file, oldFile);
                            _mainRepositoryManager.SysCustomerFileRepository.Update(oldFile);
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                    }
                }

                if (entity.CustomerContactDtos != null)
                {
                    var contacts = _mapper.Map<IEnumerable<SysCustomerContact>>(entity.CustomerContactDtos);

                    foreach (var contact in contacts)
                    {
                        if (contact.Id == 0)
                        {
                            contact.CusId = (int)customer.Id;
                            contact.Email = customer.SponsorEmail;
                            contact.Email2 = "";
                            contact.IdNo = customer.SponsorId;
                            contact.Mobile = customer.SponsorMobile;
                            contact.Name = customer.SponsorName;
                            contact.Phone = customer.SponsorPhone;
                            contact.JobAddress = customer.SponsorJobAddress;
                            contact.JobName = customer.SponsorJobName;
                            var newContact = await _mainRepositoryManager.SysCustomerContactRepository.AddAndReturn(_mapper.Map<SysCustomerContact>(contact));
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                        else
                        {
                            var oldContact = await _mainRepositoryManager.SysCustomerContactRepository.GetById(contact.Id);

                            if (oldContact == null) continue;
                            _mapper.Map(contact, oldContact);
                            contact.CusId = (int)customer.Id;
                            contact.Email = customer.SponsorEmail;
                            contact.Email2 = "";
                            contact.IdNo = customer.SponsorId;
                            contact.Mobile = customer.SponsorMobile;
                            contact.Name = customer.SponsorName;
                            contact.Phone = customer.SponsorPhone;
                            contact.JobAddress = customer.SponsorJobAddress;
                            contact.JobName = customer.SponsorJobName;
                            _mainRepositoryManager.SysCustomerContactRepository.Update(oldContact);
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                    }
                }

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<SysCustomerEditQVDto>.SuccessAsync(entityMap, localization.GetResource1("UpdateSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerEditQVDto>.FailAsync($"Update failed: {exp.Message}");
            }
        }
        public async Task<IResult<SysCustomerEditQVDto>> ApproveVendor(SysCustomerEditQVDto entity, bool approve, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));

            var customer = await _mainRepositoryManager.SysCustomerRepository.GetById(entity.Id);
            if (customer == null)
                return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetResource1("NoData"));
            try
            {
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var checkIdNo = await _mainRepositoryManager.SysCustomerRepository.AnyAsync(
                    c => c.Id != entity.Id && c.IdNo == entity.IdNo && c.FacilityId == session.FacilityId &&
                         c.CusTypeId == entity.CusTypeId && c.IsDeleted == false);

                if (checkIdNo)
                    return await Result<SysCustomerEditQVDto>.FailAsync(localization.GetResource1("IdentityExist"));

                _mapper.Map(entity, customer);
                customer.FacilityId = session.FacilityId;
                customer.Photo = "";
                customer.RepresentedBy = "";
                customer.CityId = 0;
                customer.CusTypeId = 8;
                customer.AccAccountId = 0;
                
                if(approve == true)
                {
                    customer.Id = entity.Id;
                    customer.IdNo = entity.IdNo;
                    customer.IdDate = entity.IdDate;
                    customer.IdIssuer = entity.IdIssuer;
                    customer.IdType = entity.IdType;
                    customer.JobAddress = entity.JobAddress;
                    customer.JobName = entity.JobName;
                    customer.Mobile = entity.Mobile;
                    customer.Name = entity.Name;
                    customer.NationalityId = entity.NationalityId;
                    customer.Note = entity.Note;
                    customer.Phone = entity.Phone;
                    customer.Fax = entity.Fax;
                    customer.Photo = ""; // Default reset
                    customer.RepresentedBy = ""; // Default reset
                    customer.Email = entity.Email;
                    customer.BankAccount = entity.BankAccount;
                    customer.BankId = entity.BankId;
                    customer.CityId = 0; // Default reset
                    customer.BranchId = entity.BranchId;
                    customer.ModifiedBy = entity.ModifiedBy;
                    customer.CusTypeId = 1; // Vendor type
                    customer.Address = entity.Address;
                    customer.AccAccountId = entity.AccAccountId;
                    customer.GroupId = entity.GroupId;
                    customer.CurrencyId = entity.CurrencyId;
                    customer.VatEnable = entity.VatEnable;
                    customer.VatNumber = entity.VatNumber;
                    customer.Name2 = entity.Name2;
                    customer.Enable = entity.Enable;
                    customer.CustomerName = entity.CustomerName;
                    customer.FacilityId = session.FacilityId;

                    customer.SponsorId = entity.SponsorId;
                    customer.SponsorJobAddress = entity.SponsorJobAddress;
                    customer.SponsorJobName = entity.SponsorJobName;
                    customer.SponsorMobile = entity.SponsorMobile;
                    customer.SponsorName = entity.SponsorName;
                    customer.SponsorPhone = entity.SponsorPhone;
                    customer.SponsorEmail = entity.SponsorEmail;
                }

                _mainRepositoryManager.SysCustomerRepository.Update(customer);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerEditQVDto>(customer);

                if (entity.FileDtos != null)
                {
                    var files = _mapper.Map<IEnumerable<SysCustomerFile>>(entity.FileDtos);

                    foreach (var file in files)
                    {
                        if (file.Id == 0)
                        {
                            file.CustomerId = customer.Id;
                            var newFile = await SaveCustomerFile(file);
                        }
                        else
                        {
                            file.CustomerId = customer.Id;
                            var oldFile = await _mainRepositoryManager.SysCustomerFileRepository.GetById(file.Id);

                            if (oldFile == null) continue;
                            _mapper.Map(file, oldFile);
                            _mainRepositoryManager.SysCustomerFileRepository.Update(oldFile);
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                    }
                }

                if (entity.CustomerContactDtos != null)
                {
                    var contacts = _mapper.Map<IEnumerable<SysCustomerContact>>(entity.CustomerContactDtos);

                    foreach (var contact in contacts)
                    {
                        if (contact.Id == 0)
                        {
                            contact.CusId = (int)customer.Id;
                            contact.Email = customer.SponsorEmail;
                            contact.Email2 = "";
                            contact.IdNo = customer.SponsorId;
                            contact.Mobile = customer.SponsorMobile;
                            contact.Name = customer.SponsorName;
                            contact.Phone = customer.SponsorPhone;
                            contact.JobAddress = customer.SponsorJobAddress;
                            contact.JobName = customer.SponsorJobName;
                            var newContact = await _mainRepositoryManager.SysCustomerContactRepository.AddAndReturn(_mapper.Map<SysCustomerContact>(contact));
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                        else
                        {
                            var oldContact = await _mainRepositoryManager.SysCustomerContactRepository.GetById(contact.Id);

                            if (oldContact == null) continue;
                            _mapper.Map(contact, oldContact);
                            contact.CusId = (int)customer.Id;
                            contact.Email = customer.SponsorEmail;
                            contact.Email2 = "";
                            contact.IdNo = customer.SponsorId;
                            contact.Mobile = customer.SponsorMobile;
                            contact.Name = customer.SponsorName;
                            contact.Phone = customer.SponsorPhone;
                            contact.JobAddress = customer.SponsorJobAddress;
                            contact.JobName = customer.SponsorJobName;
                            _mainRepositoryManager.SysCustomerContactRepository.Update(oldContact);
                            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                    }
                }

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<SysCustomerEditQVDto>.SuccessAsync(entityMap, localization.GetResource1("UpdateSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerEditQVDto>.FailAsync($"Update failed: {exp.Message}");
            }
        }
        //private async Task<string?> CheckUniqueFieldsAsync(string IdNo, CancellationToken cancellationToken)
        //{
        //    if (!string.IsNullOrEmpty(IdNo))
        //    {
        //        var idNoExists = await _mainRepositoryManager.SysCustomerRepository
        //            .AnyAsync(c => c.Id != customer.Id && c.IdNo == customer.IdNo && c.FacilityId == customer.FacilityId && c.CusTypeId == customer.CusTypeId && c.IsDeleted == false);
        //        if (idNoExists) return "ID Number already exists.";
        //    }
        //    return null;
        //}
        public async Task<IResult> RemoveQualifiedVendor(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerRepository.GetById(Id);
                if (item == null)
                    return Result<SysCustomerDto>.Fail(localization.GetMessagesResource("InCorrectId"));

                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                _mainRepositoryManager.SysCustomerRepository.Update(item);

                await RemoveRelatedContacts(Id, cancellationToken);
                await RemoveRelatedFiles(Id, cancellationToken);

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<SysCustomerDto>.SuccessAsync(_mapper.Map<SysCustomerDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerDto>.FailAsync($"EXP in Remove at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
        private async Task RemoveRelatedFiles(long id, CancellationToken cancellationToken)
        {
            var files = await _mainRepositoryManager.SysCustomerFileRepository.GetAll(x => x.CustomerId == id);
            if (files.Any())
            {
                await _mainRepositoryManager.SysCustomerFileRepository.RemoveFiles(id, cancellationToken);
                await accRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
            }
        }
        private async Task RemoveRelatedContacts(long id, CancellationToken cancellationToken)
        {
            var result = await _mainRepositoryManager.SysCustomerContactRepository.RemoveContacts(id, cancellationToken);
            if (result.Succeeded)
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
            }
            else
            {
                throw new Exception(result.Status.message);
            }
        }
    }
}
