using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Periods")]
    public partial class SysPeriod : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("Start_Date")]
        [StringLength(50)]
        public string? StartDate { get; set; }

        [Column("End_Date")]
        [StringLength(50)]
        public string? EndDate { get; set; }

        [Column("System_ID")]
        public long? SystemId { get; set; }

        [Column("Facility_ID")]
        public long? FacilityId { get; set; }

        [Column("Is_Active")]
        public bool? IsActive { get; set; }
    }
}
