using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_lookup_Data")]
    [Index("Code", "CatagoriesId", "Isdel", Name = "Ind_catagory", IsUnique = true)]
    
    public partial class SysLookupData
    {
        public long? Code { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Catagories_ID")]
        public int? CatagoriesId { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        [Column("Sort_no")]
        public int? SortNo { get; set; }
        public string? Note { get; set; }
        [Column("Refrance_No")]
        [StringLength(250)]
        public string? RefranceNo { get; set; }
        [Column("Color_ID")]
        public int? ColorId { get; set; }
        [StringLength(250)]
        public string? Icon { get; set; }
        [Column("Acc_Account_ID")]
        public long? AccAccountId { get; set; }
        [Column("CC_ID")]
        public long? CcId { get; set; }
        [StringLength(250)]
        public string? Name2 { get; set; }
    }
}
