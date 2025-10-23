using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Screen_Properties")]
    public partial class SysScreenProperty
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("Screen_ID")]
        public long? ScreenId { get; set; }

        [Column("Property_Name")]
        [StringLength(500)]
        public string? PropertyName { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// نوع الخاصية هل checkbox Or Value
        /// </summary>
        [Column("Property_Type")]
        public int? PropertyType { get; set; }

        public long? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public bool? IsDeleted { get; set; }

        [Column("ControlID")]
        public string? ControlId { get; set; }

        public string? Attribute { get; set; }

        public string? AttributeValue { get; set; }

        [Column("Screen_Url")]
        public string? ScreenUrl { get; set; }

        public string? NotAttributeValue { get; set; }
    }
}
