using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Logix.Domain.Main
{
    [Table("Sys_Department")]
    
    public partial class SysDepartment
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = null!;
        public long Code { get; set; }
        [Column("Parent_Id")]
        public long ParentId { get; set; }
        [StringLength(20)]
        public string? Tel { get; set; }
        [StringLength(20)]
        public string? Fax { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(50)]
        public string? Mobile { get; set; }
        [StringLength(200)]
        public string? Note { get; set; }
        [Column("Level_No")]
        public int? LevelNo { get; set; }
        [Column("Type_ID")]
        public int? TypeId { get; set; }
        [Column("City_ID")]
        public int? CityId { get; set; }
        public int? Isdel { get; set; }
        [Column("Cat_ID")]
        public int? CatId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("CC_ID")]
        public long? CcId { get; set; }
        [Column("Project_ID")]
        public long? ProjectId { get; set; }
        [StringLength(200)]
        public string? Name2 { get; set; }
        [Column("Dep_Manger_ID")]
        public long? DepMangerId { get; set; }
        [Column("Status_ID")]
        public int? StatusId { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        public bool? IsShare { get; set; }
        public string? Address { get; set; }
        [StringLength(250)]
        public string? Latitude { get; set; }
        [StringLength(250)]
        public string? Longitude { get; set; }
        [StringLength(250)]
        public string? ContactPerson { get; set; }
        [Column("CustomerID")]
        public long? CustomerId { get; set; }
        //هذا الحقل تمت اضافته ليستخدم في نظام التشغيل والصيانه.لتحديد مواقع السكن

        public bool? IsResidence { get; set; }
        [Column("BRANCH_ID")]
        public int? BranchId { get; set; }
        [Column("CC_ID2")]
        public long? CcId2 { get; set; }
        [Column("CC_ID3")]
        public long? CcId3 { get; set; }
        [Column("CC_ID4")]
        public long? CcId4 { get; set; }
        [Column("CC_ID5")]
        public long? CcId5 { get; set; }
    }
}
