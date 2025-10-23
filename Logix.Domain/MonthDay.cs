using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    [Table("Month_Day")]
    public partial class MonthDay
    {
        [Column("Day_Code")]
        [StringLength(2)]
        [Unicode(false)]
        public string? DayCode { get; set; }
    }
}
