using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Properties")]
    public partial class SysProperty
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
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
        public string? Description { get; set; }
        [Column("Classifications_ID")]
        public long? ClassificationsId { get; set; }
        [Column("Is_Required")]
        public bool? IsRequired { get; set; }
    }
}
