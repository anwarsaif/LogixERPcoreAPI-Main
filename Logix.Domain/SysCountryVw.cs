using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCountryVw
    {
        [Column("Country_ID")]
        public long? CountryId { get; set; }
        [Column("Country_Name")]
        [StringLength(250)]
        public string? CountryName { get; set; }
        [Column("Country_Name2")]
        [StringLength(250)]
        public string? CountryName2 { get; set; }
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
        [StringLength(50)]
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
    }
}
