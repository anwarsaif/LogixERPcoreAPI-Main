using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Property_Values")]
    public partial class SysPropertyValue
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Property_ID")]
        public long? PropertyId { get; set; }
        [Column("Property_Value")]
        public string? PropertyValue { get; set; }
    }
}
