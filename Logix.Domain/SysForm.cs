using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Forms")]
    public partial class SysForm : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(50)]
        public string? Name1 { get; set; }
        [Column("NAme2")]
        [StringLength(50)]
        public string? Name2 { get; set; }
        [Column("System_ID")]
        public long? SystemId { get; set; }
        [StringLength(150)]
        public string? Url { get; set; }
    }
}
