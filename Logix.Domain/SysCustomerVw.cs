using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCustomerVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Cus_Type_Id")]
        public int? CusTypeId { get; set; }
        [StringLength(250)]
        public string? Code { get; set; }
        [StringLength(2500)]
        public string? Name { get; set; }
        [Column("ID_No")]
        [StringLength(50)]
        public string? IdNo { get; set; }
        [Column("ID_Date")]
        [StringLength(50)]
        public string? IdDate { get; set; }
        [Column("ID_Type")]
        public int? IdType { get; set; }
        [Column("ID_Issuer")]
        [StringLength(50)]
        public string? IdIssuer { get; set; }
        [Column("Nationality_ID")]
        public int? NationalityId { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(50)]
        public string? Phone { get; set; }
        [StringLength(50)]
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        [StringLength(250)]
        public string? Note { get; set; }
        [Column("Represented_by")]
        [StringLength(1000)]
        public string? RepresentedBy { get; set; }
        [Column("Job_Name")]
        [StringLength(50)]
        public string? JobName { get; set; }
        [Column("Job_Address")]
        [StringLength(2500)]
        public string? JobAddress { get; set; }
        [StringLength(2500)]
        public string? Photo { get; set; }
        [Column("Sponsor_ID")]
        [StringLength(50)]
        public string? SponsorId { get; set; }
        [Column("Sponsor_Name")]
        [StringLength(1500)]
        public string? SponsorName { get; set; }
        [Column("Sponsor_Job_Name")]
        [StringLength(1500)]
        public string? SponsorJobName { get; set; }
        [Column("Sponsor_Job_Address")]
        [StringLength(2500)]
        public string? SponsorJobAddress { get; set; }
        [Column("Sponsor_Mobile")]
        [StringLength(50)]
        public string? SponsorMobile { get; set; }
        [Column("Sponsor_Phone")]
        [StringLength(50)]
        public string? SponsorPhone { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Branch_ID")]
        public int? BranchId { get; set; }
        [Column("Acc_Account_ID")]
        public long? AccAccountId { get; set; }
        [Column("Acc_Account_Code")]
        [StringLength(50)]
        public string? AccAccountCode { get; set; }
        [Column("Acc_Account_Name")]
        [StringLength(255)]
        public string? AccAccountName { get; set; }
        [Column("Bank_Account")]
        [StringLength(50)]
        public string? BankAccount { get; set; }
        [Column("Bank_ID")]
        public int? BankId { get; set; }
        [StringLength(50)]
        public string? Fax { get; set; }
        [Column("Customer_Name")]
        [StringLength(250)]
        public string? CustomerName { get; set; }
        [StringLength(50)]
        public string? Email2 { get; set; }
        [Column("Acc_separate")]
        public bool? AccSeparate { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("City_ID")]
        public int? CityId { get; set; }
        [Column("Credit_limit", TypeName = "decimal(18, 2)")]
        public decimal? CreditLimit { get; set; }
        [Column("Group_ID")]
        public int? GroupId { get; set; }
        [Column("Comany_Type")]
        public int? ComanyType { get; set; }
        [Column("Currency_ID")]
        public int? CurrencyId { get; set; }
        [Column("Emp_ID")]
        public long? EmpId { get; set; }
        [Column("Emp_Code")]
        [StringLength(50)]
        public string? EmpCode { get; set; }
        [Column("Emp_name")]
        [StringLength(250)]
        public string? EmpName { get; set; }
        [Column("Item_Price_M_ID")]
        public long? ItemPriceMId { get; set; }
        [Column("ID_Type_Name")]
        [StringLength(250)]
        public string? IdTypeName { get; set; }
        [Column("Source_ID")]
        public int? SourceId { get; set; }
        [Column("Status_ID")]
        public int? StatusId { get; set; }
        [Column("Share_with_users")]
        public string? ShareWithUsers { get; set; }
        [Column("Industry_ID")]
        public int? IndustryId { get; set; }
        [Column("Number_of_Employees")]
        [StringLength(2500)]
        public string? NumberOfEmployees { get; set; }
        [Column("Status_Name")]
        [StringLength(250)]
        public string? StatusName { get; set; }
        [Column("Sponsor_Email")]
        [StringLength(50)]
        public string? SponsorEmail { get; set; }
        [Column("VAT_Enable")]
        public bool? VatEnable { get; set; }
        [Column("VAT_Number")]
        [StringLength(250)]
        public string? VatNumber { get; set; }
        [StringLength(2550)]
        public string? Name2 { get; set; }
        [Column("Due_Period_Days")]
        public int? DuePeriodDays { get; set; }
        [Column("Collector_Name")]
        [StringLength(250)]
        public string? CollectorName { get; set; }
        [Column("Created_Date")]
        [StringLength(10)]
        public string? CreatedDate { get; set; }
        [StringLength(2400)]
        public string? Latitude { get; set; }
        [StringLength(2400)]
        public string? Longitude { get; set; }
        [Column("Safety_Period_Days")]
        public int? SafetyPeriodDays { get; set; }
        public int? Enable { get; set; }
        [Column("City_Code")]
        [StringLength(50)]
        public string? CityCode { get; set; }
        [Column("City_Name")]
        [StringLength(500)]
        public string? CityName { get; set; }
        [Column("Payment_Type")]
        public int? PaymentType { get; set; }
        [Column("Sales_Type")]
        public int? SalesType { get; set; }
        [Column("Sales_Area")]
        public int? SalesArea { get; set; }
        [Column("POS_ID")]
        public int? PosId { get; set; }
        [Column("Days_of_Visit")]
        [StringLength(50)]
        public string? DaysOfVisit { get; set; }
        [Column("Sales_Area_Code")]
        [StringLength(50)]
        public string? SalesAreaCode { get; set; }
        [Column("Sales_Area_Name")]
        [StringLength(500)]
        public string? SalesAreaName { get; set; }
        public int? Gender { get; set; }
        [Column("Refrance_Code")]
        [StringLength(250)]
        public string? RefranceCode { get; set; }
        [Column("Member_ID")]
        [StringLength(250)]
        public string? MemberId { get; set; }
        [Column("Frist_Name")]
        [StringLength(250)]
        public string? FristName { get; set; }
        [Column("Second_Name")]
        [StringLength(250)]
        public string? SecondName { get; set; }
        [Column("Third_Name")]
        [StringLength(250)]
        public string? ThirdName { get; set; }
        [Column("Fourth_Name")]
        [StringLength(250)]
        public string? FourthName { get; set; }
        [Column("Title_ID")]
        public int? TitleId { get; set; }
        [StringLength(250)]
        public string? Veneration { get; set; }
        [Column("Contact_By")]
        [StringLength(250)]
        public string? ContactBy { get; set; }
        [Column("BRA_NAME")]
        public string? BraName { get; set; }
        [Column("Nationality_Name")]
        [StringLength(250)]
        public string? NationalityName { get; set; }
        [Column("Company_Type")]
        [StringLength(50)]
        public string? CompanyType { get; set; }
        [StringLength(250)]
        public string? Title { get; set; }
        [StringLength(10)]
        public string? DateOfBirth { get; set; }
        [StringLength(57)]
        public string? Duration { get; set; }
        [Column("Status_Color")]
        [StringLength(250)]
        public string? StatusColor { get; set; }
        [Column("Status_Color_Value")]
        [StringLength(250)]
        public string? StatusColorValue { get; set; }
        [Column("Status_Icon")]
        [StringLength(250)]
        public string? StatusIcon { get; set; }
        [Column("Parent_ID")]
        public long? ParentId { get; set; }
        [Column("Parent_RelativeType")]
        public int? ParentRelativeType { get; set; }
        [Column("Parent_Code")]
        [StringLength(250)]
        public string? ParentCode { get; set; }
        [Column("Parent_Name")]
        [StringLength(2500)]
        public string? ParentName { get; set; }
        [Column("Parent_ID_No")]
        [StringLength(50)]
        public string? ParentIdNo { get; set; }
        [Column("Parent_Mobile")]
        [StringLength(50)]
        public string? ParentMobile { get; set; }
        [Column("Std_Status_ID")]
        public int? StdStatusId { get; set; }
        [Column("Std_Status_Name")]
        [StringLength(250)]
        public string? StdStatusName { get; set; }
        [Column("Std_Status_Name2")]
        [StringLength(250)]
        public string? StdStatusName2 { get; set; }
        [Column("Std_Level_ID")]
        public long? StdLevelId { get; set; }
        [Column("Std_Grade_ID")]
        public int? StdGradeId { get; set; }
        [Column("Std_Grade_Code")]
        [StringLength(250)]
        public string? StdGradeCode { get; set; }
        [Column("Std_Grade_Name")]
        [StringLength(250)]
        public string? StdGradeName { get; set; }
        [Column("Std_Level_Name")]
        [StringLength(250)]
        public string? StdLevelName { get; set; }
        [Column("Std_Level_Name2")]
        [StringLength(250)]
        public string? StdLevelName2 { get; set; }
        [Column("Parent_Nationality_Name")]
        [StringLength(250)]
        public string? ParentNationalityName { get; set; }
        [Column("Status_Name2")]
        [StringLength(250)]
        public string? StatusName2 { get; set; }
        [Column("BRA_NAME2")]
        public string? BraName2 { get; set; }
        [Column("App_ID")]
        public long? AppId { get; set; }
        [Column("App_status_Name")]
        [StringLength(50)]
        public string? AppStatusName { get; set; }
        [Column("App_Step_Name")]
        [StringLength(250)]
        public string? AppStepName { get; set; }
        [Column("Application_Code")]
        public long? ApplicationCode { get; set; }
        [Column("App_Step_Name2")]
        [StringLength(250)]
        public string? AppStepName2 { get; set; }
        [Column("App_status_Name2")]
        [StringLength(50)]
        public string? AppStatusName2 { get; set; }
        [Column("App_Status_ID")]
        public int? AppStatusId { get; set; }
        [Column("App_Step_ID")]
        public int? AppStepId { get; set; }
        [Column("University_ID")]
        [StringLength(50)]
        public string? UniversityId { get; set; }
        [Column("ID_Expire_Date")]
        [StringLength(10)]
        public string? IdExpireDate { get; set; }
        [Column("Owner_ID_No")]
        [StringLength(50)]
        public string? OwnerIdNo { get; set; }
        [Column("Free_Maintenance")]
        public bool? FreeMaintenance { get; set; }
        [Column("Preventive_Maintenance")]
        public bool? PreventiveMaintenance { get; set; }
        [Column("Correctional_Maintenance")]
        public bool? CorrectionalMaintenance { get; set; }
        [Column("JobID")]
        public long? JobId { get; set; }
        public string? Vission { get; set; }
        public string? Mission { get; set; }
        public string? Objective { get; set; }
        [Column("ISBusinessPartner")]
        public int? IsbusinessPartner { get; set; }
        public string? AcademicDegree { get; set; }
        [Column("Industry_Name")]
        [StringLength(250)]
        public string? IndustryName { get; set; }
        [Column("Industry_Name2")]
        [StringLength(250)]
        public string? IndustryName2 { get; set; }
        [Column("Parent_Name2")]
        [StringLength(2550)]
        public string? ParentName2 { get; set; }
        public int? RateFileCompletion { get; set; }
        [Column("ISCompleted")]
        public bool? Iscompleted { get; set; }
        [Column("ISCompletedFile")]
        [StringLength(9)]
        public string IscompletedFile { get; set; } = null!;
        [Column("Attachment_IBN")]
        [StringLength(500)]
        public string? AttachmentIbn { get; set; }
        [Column("Attachment_Profile")]
        [StringLength(500)]
        public string? AttachmentProfile { get; set; }
        [Column("Count_Employee_primary")]
        [StringLength(250)]
        public string? CountEmployeePrimary { get; set; }
        [Column("Count_Employee_foreign")]
        [StringLength(250)]
        public string? CountEmployeeForeign { get; set; }
        [Column("Attachment_Organizationalchart")]
        [StringLength(500)]
        public string? AttachmentOrganizationalchart { get; set; }
        public string? OtherDetails { get; set; }
        [Column("Sponsor_POBox")]
        [StringLength(500)]
        public string? SponsorPobox { get; set; }
        [Column("Sponsor_ZipCode")]
        [StringLength(500)]
        public string? SponsorZipCode { get; set; }
        [Column("Sponsor_Attachment")]
        [StringLength(500)]
        public string? SponsorAttachment { get; set; }
        [Column("POBox")]
        [StringLength(500)]
        public string? Pobox { get; set; }
        [StringLength(500)]
        public string? ZipCode { get; set; }
        [StringLength(500)]
        public string? Attachment { get; set; }
        public bool? PortalCondition { get; set; }
        [Column("PortalCusType_Id")]
        public int? PortalCusTypeId { get; set; }
        [Column("Owner_Property")]
        public bool? OwnerProperty { get; set; }
    }
}
