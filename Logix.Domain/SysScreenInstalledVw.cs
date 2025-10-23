using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysScreenInstalledVw
    {
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        [Column("Parent_Id")]
        public int? ParentId { get; set; }
        [Column("Sort_no")]
        public int? SortNo { get; set; }
        [Column("SCREEN_URL")]
        public string? ScreenUrl { get; set; }
        [Column("Icon_Css")]
        [StringLength(50)]
        public string? IconCss { get; set; }
        [Column("Color_Css")]
        [StringLength(2500)]
        public string? ColorCss { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("ID")]
        public long Id { get; set; }
    }
}
