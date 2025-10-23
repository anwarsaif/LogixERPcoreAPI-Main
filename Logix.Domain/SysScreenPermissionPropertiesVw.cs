using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysScreenPermissionPropertiesVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Property_ID")]
        public long? PropertyId { get; set; }
        public bool? Allow { get; set; }
        public string? Value { get; set; }
        [Column("User_ID")]
        public long? UserId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [Column("Screen_Url")]
        public string? ScreenUrl { get; set; }
        public string? AttributeValue { get; set; }
        public string? Attribute { get; set; }
        public bool? IsDeletedM { get; set; }
        [Column("ControlID")]
        public string? ControlId { get; set; }
        public string? NotAttributeValue { get; set; }
    }
}
