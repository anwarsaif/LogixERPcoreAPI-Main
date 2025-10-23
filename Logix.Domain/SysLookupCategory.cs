using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_LookUp_Catagories")]
    public partial class SysLookupCategory 
    {
        [Key]
        [Column("Catagories_ID")]
        public long CatagoriesId { get; set; }
        [Column("Catagories_Name")]
        [StringLength(250)]
        public string? CatagoriesName { get; set; }
        [Column("Catagories_name2")]
        [StringLength(250)]
        public string? CatagoriesName2 { get; set; }
        [Column("System_ID")]
        [StringLength(500)]
        public string? SystemId { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDeletable { get; set; }
    }
}
