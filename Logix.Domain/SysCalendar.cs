using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Calendar")]
    public partial class SysCalendar
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("G_Date")]
        [StringLength(10)]
        public string? GDate { get; set; }
        [Column("H_Date")]
        [StringLength(10)]
        public string? HDate { get; set; }
    }
}
