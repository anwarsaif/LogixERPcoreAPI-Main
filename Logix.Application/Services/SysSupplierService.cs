using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace Logix.Application.Services.Main
{
    public class SysSupplierService : GenericQueryService<SysCustomer, SysCustomerDto, SysCustomerVw>, ISysSupplierService
    {
        private readonly IAccRepositoryManager accRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly ILocalizationService localization;
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly IWebHostEnvironment env;

        public SysSupplierService(IQueryRepository<SysCustomer> queryRepository, IAccRepositoryManager accRepositoryManager, IHrRepositoryManager hrRepositoryManager, ILocalizationService localization, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, IWebHostEnvironment env) : base(queryRepository, mapper)
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

                // ================= validations befor add  ====================
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
                        var accBranch = await accRepositoryManager.AccBranchAccountRepository.GetOneVW(s => s.IsDeleted == false && s.BranchId == (long)(item.BranchId ?? 0) && s.Name2 == BranchAccountType.Supplier.ToString());
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
                item.CusTypeId = 1;
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

                //------ check phone ----------------
                var chkPhone = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Phone != null && s.Phone == item.Phone && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                if (chkPhone != null && chkPhone.Count() > 0)
                {
                    return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("رقم الهاتف موجود مسبقا"));
                }

                //------ check mobile ----------------
                var chkMobile = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Mobile != null && s.Mobile == item.Mobile && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                if (chkMobile != null && chkMobile.Count() > 0)
                {
                    return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("MobileAlreadyExists"));
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
                if (item.EmpId != null)
                {
                    var chkEmp = await hrRepositoryManager.HrEmployeeRepository.GetOne(e => e.EmpId == item.EmpId.ToString());
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
                        return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("SEnterSuppliercodeFirst"));
                    }
                    else
                    {
                        var codeExists = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Code == item.Code && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
                        if (chkMobile != null && chkMobile.Count() > 0)
                        {
                            return await Result<SysCustomerAddVM>.FailAsync(localization.GetSALResource("رقم المورد موجود مسبقاً"));
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
                        var contact = _mapper.Map<SysCustomerContact>(entity.SysCustomerContactDto);
                        contact.Name = item.SponsorName;
                        contact.Email = item.SponsorEmail;
                        contact.Phone = item.SponsorPhone;
                        contact.Mobile = item.SponsorMobile;
                        contact.AcademicDegree = item.AcademicDegree;
                        contact.IdNo = item.SponsorId;
                        contact.JobName = item.SponsorJobName;
                        contact.JobAddress = item.SponsorJobAddress;
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

            var item = await _mainRepositoryManager.SysCustomerRepository.GetById(entity.Id??0);

            if (item == null) return await Result<SysCustomerEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

            _mapper.Map(entity, item);

            item.CusTypeId = 1;
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

            //------ check phone ----------------
            var chkPhone = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Id != item.Id && s.Phone != null && s.Phone == item.Phone && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
            if (chkPhone != null && chkPhone.Count() > 0)
            {
                return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("رقم الهاتف موجود مسبقا"));
            }

            //------ check mobile ----------------
            var chkMobile = await _mainRepositoryManager.SysCustomerRepository.GetAll(s => s.Id != item.Id && s.Mobile == item.Mobile && s.FacilityId == item.FacilityId && s.CusTypeId == item.CusTypeId && s.IsDeleted != true, s => s.Id);
            if (chkMobile != null && chkMobile.Count() > 0)
            {
                return await Result<SysCustomerEditDto>.FailAsync(localization.GetSALResource("MobileAlreadyExists"));
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
            if (item.EmpId != null)
            {
                var chkEmp = await hrRepositoryManager.HrEmployeeRepository.GetOne(e => e.EmpId == item.EmpId.ToString());
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
                if (item == null) return Result<SysCustomerDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                // in controller, we check if this supplier can be removed or not

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
                if (item == null) return Result<SysCustomerDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                // in controller, we check if this supplier can be removed or not

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
    }
}
