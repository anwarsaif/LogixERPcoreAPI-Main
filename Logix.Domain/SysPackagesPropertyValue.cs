using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Packages_Property_Values")]
    public partial class SysPackagesPropertyValue
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("Package_ID")]
        public long? PackageId { get; set; }

        [Column("Property_ID")]
        public long? PropertyId { get; set; }

        [Column("Facility_ID")]
        public long? FacilityId { get; set; }

        [Column("Property_Value")]
        public string? PropertyValue { get; set; }

        public long CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}