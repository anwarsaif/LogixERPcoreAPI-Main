using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_ActivityType")]
    public partial class SysActivityType
    {
        [Key]
        [Column("ActivityTypeID")]
        public int ActivityTypeId { get; set; }

        [StringLength(50)]
        public string? ActivityType { get; set; }
    }
}
