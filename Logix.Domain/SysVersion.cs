using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Versions")]
    public partial class SysVersion
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [Column("Version_No")]
        [StringLength(50)]
        public string? VersionNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
    }
}
