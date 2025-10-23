using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{

    [Keyless]
    public partial class SysDepartmentVw
    {
        [Column("ID")]
        public long Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Name2 { get; set; }

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

        [Column("Dep_Manger_ID")]
        public long? DepMangerId { get; set; }

        [Column("CostCenter_Code")]
        [StringLength(50)]
        public string? CostCenterCode { get; set; }

        [Column("CostCenter_Name")]
        [StringLength(150)]
        public string? CostCenterName { get; set; }

        [Column("Emp_Code")]
        [StringLength(50)]
        public string? EmpCode { get; set; }

        [Column("Emp_name")]
        [StringLength(250)]
        public string? EmpName { get; set; }

        [Column("Emp_Photo")]
        [StringLength(500)]
        public string? EmpPhoto { get; set; }

        [Column("Project_Code")]
        public long? ProjectCode { get; set; }

        [Column("Project_Name")]
        [StringLength(2500)]
        public string? ProjectName { get; set; }

        [Column("Customer_ID")]
        public long? CustomerId { get; set; }

        [Column("BRANCH_ID")]
        public int? BranchId { get; set; }

        [Column("Project_Start")]
        [StringLength(10)]
        public string? ProjectStart { get; set; }

        [Column("Project_END")]
        [StringLength(10)]
        public string? ProjectEnd { get; set; }

        [Column("Status_ID")]
        public int? StatusId { get; set; }

        [Column("Facility_ID")]
        public long? FacilityId { get; set; }

        public bool? IsShare { get; set; }

        [Column("Emp_name2")]
        [StringLength(250)]
        public string? EmpName2 { get; set; }

        [Column("Structure_ID")]
        public int? StructureId { get; set; }

        [Column("Structure_Name")]
        [StringLength(250)]
        public string? StructureName { get; set; }

        [Column("Structure_Name2")]
        [StringLength(250)]
        public string? StructureName2 { get; set; }

        [Column("Cat_Name")]
        [StringLength(250)]
        public string? CatName { get; set; }

        [Column("Cat_Name2")]
        [StringLength(250)]
        public string? CatName2 { get; set; }
    }
}
