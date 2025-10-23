using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysTemplateVw
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("NAME")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Column("SCREEN_ID")]
        public long? ScreenId { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("DETAILES", TypeName = "ntext")]
        public string? Detailes { get; set; }
        [Column("Type_ID")]
        public int? TypeId { get; set; }
        [Column("System_Name2")]
        [StringLength(50)]
        public string? SystemName2 { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
        public string? Url { get; set; }
    }
}
