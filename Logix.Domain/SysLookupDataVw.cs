using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysLookupDataVw
    {
        public long? Code { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string? Name2 { get; set; }
        [Column("ID")]
        public long Id { get; set; }
        [Column("Catagories_Name")]
        [StringLength(250)]
        public string? CatagoriesName { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("Catagories_ID")]
        public int? CatagoriesId { get; set; }
        public string? Note { get; set; }
        [Column("System_ID")]
        [StringLength(500)]
        public string? SystemId { get; set; }
        [Column("Refrance_No")]
        [StringLength(250)]
        public string? RefranceNo { get; set; }
        [Column("Color_ID")]
        public int? ColorId { get; set; }
        [StringLength(250)]
        public string? Icon { get; set; }
        [Column("Acc_Account_ID")]
        public long? AccAccountId { get; set; }
        [Column("Acc_Account_Code")]
        [StringLength(50)]
        public string? AccAccountCode { get; set; }
        [Column("Acc_Account_Name")]
        [StringLength(255)]
        public string? AccAccountName { get; set; }
        [Column("CC_ID")]
        public long? CcId { get; set; }
        [Column("CostCenter_Code")]
        [StringLength(50)]
        public string? CostCenterCode { get; set; }
        [Column("CostCenter_Name")]
        [StringLength(150)]
        public string? CostCenterName { get; set; }
        [Column("Sort_no")]
        public int? SortNo { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        [Column("Catagories_name2")]
        [StringLength(250)]
        public string? CatagoriesName2 { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDeletable { get; set; }
    }
}
