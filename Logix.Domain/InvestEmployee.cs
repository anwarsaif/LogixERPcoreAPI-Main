using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;



[Table("INVEST_Employee")]
[Index("EmpId", Name = "Emp_ID", IsUnique = true)]
public partial class InvestEmployee
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("Emp_ID")]
    [StringLength(50)]
    public string EmpId { get; set; } = null!;

    [Column("Emp_name")]
    [StringLength(250)]
    public string? EmpName { get; set; }

    [Column("Emp_name2")]
    [StringLength(250)]
    public string? EmpName2 { get; set; }

    [Column("ISDEL")]
    public bool? Isdel { get; set; }

    [Column("USER_ID")]
    public long? UserId { get; set; }

    [Column("BRANCH_ID")]
    public int? BranchId { get; set; }

    [Column("Job_Type")]
    public int? JobType { get; set; }

    [Column("Job_Catagories_ID")]
    public int? JobCatagoriesId { get; set; }

    [Column("Status_ID")]
    public int? StatusId { get; set; }

    [Column("Job_Description")]
    public string? JobDescription { get; set; }

    [Column("Nationality_ID")]
    public int? NationalityId { get; set; }

    [Column("Marital_Status")]
    public int? MaritalStatus { get; set; }

    public int? Gender { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Salary { get; set; }

    [Column("Stop_salary")]
    public bool? StopSalary { get; set; }

    [Column("Stop_Date_Salary")]
    [StringLength(12)]
    public string? StopDateSalary { get; set; }

    [Column("Stop_Salary_Code")]
    public int? StopSalaryCode { get; set; }

    [Column("Postal_Code")]
    [StringLength(20)]
    public string? PostalCode { get; set; }

    [Column("POBox")]
    [StringLength(20)]
    public string? Pobox { get; set; }

    [Column("Home_Phone")]
    [StringLength(20)]
    public string? HomePhone { get; set; }

    [Column("Office_Phone")]
    [StringLength(20)]
    public string? OfficePhone { get; set; }

    [Column("Office_Phone_Ex")]
    [StringLength(20)]
    public string? OfficePhoneEx { get; set; }

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [Column("Emp_Photo")]
    [StringLength(500)]
    public string? EmpPhoto { get; set; }

    [Column("Contract_Type_ID")]
    public int? ContractTypeId { get; set; }

    [Column("DOAppointment")]
    [StringLength(12)]
    public string? Doappointment { get; set; }

    public string? Note { get; set; }

    [Column("ID_No")]
    [StringLength(50)]
    public string? IdNo { get; set; }

    [Column("ID_Issuer")]
    [StringLength(250)]
    public string? IdIssuer { get; set; }

    [Column("ID_Issuer_Date")]
    [StringLength(10)]
    [Unicode(false)]
    public string? IdIssuerDate { get; set; }

    [Column("ID_Expire_Date")]
    [StringLength(10)]
    [Unicode(false)]
    public string? IdExpireDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? BirthDate { get; set; }

    [Column("Birth_Place")]
    [StringLength(500)]
    public string? BirthPlace { get; set; }

    [Column("Passport_No")]
    [StringLength(50)]
    public string? PassportNo { get; set; }

    [Column("Pass_Issuer_Date")]
    [StringLength(50)]
    public string? PassIssuerDate { get; set; }

    [Column("Pass_Expire_Date")]
    [StringLength(50)]
    public string? PassExpireDate { get; set; }

    [Column("Qualification_ID")]
    public int? QualificationId { get; set; }

    [Column("Specialization_ID")]
    public int? SpecializationId { get; set; }

    [Column("Direct_Deposit")]
    public bool? DirectDeposit { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    [Column("Bank_ID")]
    public int? BankId { get; set; }

    [Column("Account_No")]
    [StringLength(50)]
    public string? AccountNo { get; set; }

    [Column("IBAN")]
    [StringLength(50)]
    public string? Iban { get; set; }

    [Column("Daily_Working_hours", TypeName = "decimal(18, 2)")]
    public decimal? DailyWorkingHours { get; set; }

    [Column("Dept_ID")]
    public int? DeptId { get; set; }

    [Column("Exclude_Attend")]
    public bool? ExcludeAttend { get; set; }

    [Column("Vacation_Days_Year")]
    public int? VacationDaysYear { get; set; }

    [Column("Pass_Issuer")]
    [StringLength(50)]
    public string? PassIssuer { get; set; }

    [Column("Religion_ID")]
    public int? ReligionId { get; set; }

    [Column("Entry_NO")]
    [StringLength(50)]
    public string? EntryNo { get; set; }

    [Column("Entry_Date")]
    [StringLength(10)]
    public string? EntryDate { get; set; }

    [Column("Entry_Port")]
    [StringLength(50)]
    public string? EntryPort { get; set; }

    [Column("Cheque_Cash")]
    public int? ChequeCash { get; set; }

    [Column("Contarct_Date")]
    [StringLength(10)]
    public string? ContarctDate { get; set; }

    [Column("ID_Type")]
    public int? IdType { get; set; }

    [Column("Work_No")]
    [StringLength(50)]
    public string? WorkNo { get; set; }

    [Column("Work_Date")]
    [StringLength(10)]
    public string? WorkDate { get; set; }

    [Column("Work_ExpDate")]
    [StringLength(50)]
    public string? WorkExpDate { get; set; }

    [Column("Work_Place")]
    [StringLength(50)]
    public string? WorkPlace { get; set; }

    [Column("Visa_No")]
    [StringLength(50)]
    public string? VisaNo { get; set; }

    [Column("CC_ID")]
    public long? CcId { get; set; }

    [Column("Account_ID")]
    public long? AccountId { get; set; }

    [Column("Account_Code")]
    [StringLength(50)]
    public string? AccountCode { get; set; }

    [Column("DOAppointmentold")]
    [StringLength(50)]
    public string? Doappointmentold { get; set; }

    [Column("Insurance_Category")]
    public int? InsuranceCategory { get; set; }

    [Column("Insurance_Company")]
    public int? InsuranceCompany { get; set; }

    [Column("Insurance_Date_Validity")]
    [StringLength(10)]
    public string? InsuranceDateValidity { get; set; }

    public int? Location { get; set; }

    [Column("Contract_Data")]
    [StringLength(10)]
    public string? ContractData { get; set; }

    [Column("Contract_expiry_Date")]
    [StringLength(10)]
    public string? ContractExpiryDate { get; set; }

    [Column("Note_Contract")]
    public string? NoteContract { get; set; }

    [Column("Emp_Code2")]
    [StringLength(50)]
    public string? EmpCode2 { get; set; }

    [Column("Gosi_Date")]
    [StringLength(10)]
    public string? GosiDate { get; set; }

    [Column("Gosi_No")]
    [StringLength(50)]
    public string? GosiNo { get; set; }

    [Column("Gosi_Salary", TypeName = "decimal(18, 2)")]
    public decimal? GosiSalary { get; set; }

    [Column("Occupation_ID")]
    [StringLength(50)]
    public string? OccupationId { get; set; }

    [Column("Sponsors_ID")]
    public int? SponsorsId { get; set; }

    [Column("Phone_Country")]
    [StringLength(50)]
    public string? PhoneCountry { get; set; }

    [Column("Address_Country")]
    [StringLength(250)]
    public string? AddressCountry { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    [Column("Insurance_Card_No")]
    [StringLength(50)]
    public string? InsuranceCardNo { get; set; }

    [Column("Ticket_to")]
    [StringLength(250)]
    public string? TicketTo { get; set; }

    [Column("Ticket_Type")]
    public int? TicketType { get; set; }

    [Column("Ticket_No_Dependent")]
    [StringLength(50)]
    public string? TicketNoDependent { get; set; }

    [Column("Salary_Group_ID")]
    public long? SalaryGroupId { get; set; }

    [Column("Facility_ID")]
    public int? FacilityId { get; set; }

    [Column("Card_Expiration_Date")]
    [StringLength(10)]
    public string? CardExpirationDate { get; set; }

    [Column("IS_Ticket")]
    public bool? IsTicket { get; set; }

    [Column("Gois_Subscription_Expiry_Date")]
    [StringLength(10)]
    public string? GoisSubscriptionExpiryDate { get; set; }

    [Column("Gosi_Bisc_Salary", TypeName = "decimal(18, 2)")]
    public decimal? GosiBiscSalary { get; set; }

    [Column("Gosi_House_Allowance", TypeName = "decimal(18, 2)")]
    public decimal? GosiHouseAllowance { get; set; }

    [Column("Gosi_Allowance_Commission", TypeName = "decimal(18, 2)")]
    public decimal? GosiAllowanceCommission { get; set; }

    [Column("Gosi_Other_Allowances", TypeName = "decimal(18, 2)")]
    public decimal? GosiOtherAllowances { get; set; }

    [Column("Place_Attendance")]
    public int? PlaceAttendance { get; set; }

    [Column("Attendance_Type")]
    public int? AttendanceType { get; set; }

    [Column("Value_Ticket", TypeName = "decimal(18, 2)")]
    public decimal? ValueTicket { get; set; }

    [Column("Ticket_Entitlement")]
    public int? TicketEntitlement { get; set; }

    [Column("Program_ID")]
    public int? ProgramId { get; set; }

    [Column("Manager_ID")]
    public long? ManagerId { get; set; }

    [Column("Others_Requirements")]
    public string? OthersRequirements { get; set; }

    [Column("Have_Bank_Loan")]
    public bool? HaveBankLoan { get; set; }

    [Column("Is_Sub")]
    public bool? IsSub { get; set; }

    [Column("Parent_ID")]
    public long? ParentId { get; set; }

    [Column("Apply_Salary_ladder")]
    public bool? ApplySalaryLadder { get; set; }

    [Column("Level_ID")]
    public int? LevelId { get; set; }

    [Column("Degree_ID")]
    public int? DegreeId { get; set; }

    [Column("Payment_Type_ID")]
    public int? PaymentTypeId { get; set; }

    [Column("Wages_Protection")]
    public int? WagesProtection { get; set; }

    [Column("Manager2_ID")]
    public long? Manager2Id { get; set; }

    [Column("Manager3_ID")]
    public long? Manager3Id { get; set; }

    [Column("Trial_Expiry_Date")]
    [StringLength(10)]
    public string? TrialExpiryDate { get; set; }

    [Column("Trial_Status_ID")]
    public int? TrialStatusId { get; set; }

    [Column("Gosi_Rate_Facility", TypeName = "decimal(18, 2)")]
    public decimal? GosiRateFacility { get; set; }

    [Column("Gosi_Type")]
    public int? GosiType { get; set; }

    [Column("Vacation2_Days_Year", TypeName = "decimal(18, 2)")]
    public decimal? Vacation2DaysYear { get; set; }

    [Column("Job_ID")]
    public long? JobId { get; set; }

    [Column("Reason_Status")]
    public string? ReasonStatus { get; set; }

    public bool? Synced { get; set; }

    [Column("Key_Check_Device")]
    [StringLength(50)]
    public string? KeyCheckDevice { get; set; }

    [Column("Check_Device")]
    public bool? CheckDevice { get; set; }

    [Column("Check_Device_Active")]
    public bool? CheckDeviceActive { get; set; }

    public int? AnnualIncreaseMethod { get; set; }

    [StringLength(10)]
    public string? LastIncrementDate { get; set; }

    [StringLength(10)]
    public string? LastPromotionDate { get; set; }

    [Column("Share_ContactInfo")]
    public bool? ShareContactInfo { get; set; }

    [Column("Payroll_Type")]
    public int? PayrollType { get; set; }

    [Column("Hour_Cost", TypeName = "decimal(18, 2)")]
    public decimal? HourCost { get; set; }

    public bool? UseIncomeTaxCalc { get; set; }

    public int? TaxId { get; set; }

    [StringLength(50)]
    public string? TaxCode { get; set; }

    [Column("IsRequiredGPS")]
    public bool? IsRequiredGps { get; set; }

    [Column("CC_ID2")]
    public long? CcId2 { get; set; }

    [Column("CC_ID3")]
    public long? CcId3 { get; set; }

    [Column("CC_ID4")]
    public long? CcId4 { get; set; }

    [Column("CC_ID5")]
    public long? CcId5 { get; set; }

    [Column("CC_Rate", TypeName = "decimal(18, 2)")]
    public decimal? CcRate { get; set; }

    [Column("CC_Rate2", TypeName = "decimal(18, 2)")]
    public decimal? CcRate2 { get; set; }

    [Column("CC_Rate3", TypeName = "decimal(18, 2)")]
    public decimal? CcRate3 { get; set; }

    [Column("CC_Rate4", TypeName = "decimal(18, 2)")]
    public decimal? CcRate4 { get; set; }

    [Column("CC_Rate5", TypeName = "decimal(18, 2)")]
    public decimal? CcRate5 { get; set; }

    [Column("TimeZone_ID")]
    public int? TimeZoneId { get; set; }

    public long? SupplierId { get; set; }

    [Column("salary_insurance_wage", TypeName = "decimal(18, 2)")]
    public decimal? SalaryInsuranceWage { get; set; }

    [Column("IsJoinedGOSIAfterJuly32024")]
    public bool? IsJoinedGosiafterJuly32024 { get; set; }

    [StringLength(50)]
    public string? Email2 { get; set; }

    [Column("First_Name")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column("Father_Name")]
    [StringLength(50)]
    public string? FatherName { get; set; }

    [Column("Grandfather_Name")]
    [StringLength(50)]
    public string? GrandfatherName { get; set; }

    [Column("Last_Name")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Column("First_Name2")]
    [StringLength(50)]
    public string? FirstName2 { get; set; }

    [Column("Father_Name2")]
    [StringLength(50)]
    public string? FatherName2 { get; set; }

    [Column("Grandfather_Name2")]
    [StringLength(50)]
    public string? GrandfatherName2 { get; set; }

    [Column("Last_Name2")]
    [StringLength(50)]
    public string? LastName2 { get; set; }

    [Column("Mobile_Backup")]
    [StringLength(20)]
    public string? MobileBackup { get; set; }

    [Column("Contract_Period_Type")]
    public int? ContractPeriodType { get; set; }

    [Column("Sector_ID")]
    public int? SectorId { get; set; }

    [Column("Blood_Type")]
    public int? BloodType { get; set; }

    [Column("Direct_Phone_Number")]
    public string? DirectPhoneNumber { get; set; }
}

