using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysPropertyValuesVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Property_ID")]
        public long? PropertyId { get; set; }
        [Column("Property_Value")]
        public string? PropertyValue { get; set; }
        [Column("Property_Code")]
        public long? PropertyCode { get; set; }
        [Column("Property_Name")]
        [StringLength(2500)]
        public string? PropertyName { get; set; }
        [Column("System_ID")]
        public int? SystemId { get; set; }
        [Column("Data_Type")]
        public int? DataType { get; set; }
        [Column("Lookup_Category_ID")]
        public int? LookupCategoryId { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [Column("System_Name2")]
        [StringLength(50)]
        public string? SystemName2 { get; set; }
        public string? Description { get; set; }
    }
}
