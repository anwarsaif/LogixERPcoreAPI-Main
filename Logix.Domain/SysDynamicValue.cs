using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_DynamicValues", Schema = "dbo")]
    public partial class SysDynamicValue
    {
        [Column("ID")]
        public long Id { get; set; }
        [Key]
        public Guid DynamicValueId { get; set; }
        public Guid? AttributeId { get; set; }
        [Column(TypeName = "sql_variant")]
        public object? DynamicValue { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [Column("Application_ID")]
        public long? ApplicationId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
