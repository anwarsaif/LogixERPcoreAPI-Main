using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logix.Domain.Main
{
    // it has to inherit from TraceEntity ==============
    [Table("Sys_Screen")]
    public partial class SysScreen
    {
        [Key]
        [Column("SCREEN_ID")]
        public long ScreenId { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
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

        //================== added by najmaldeen, Url => to save the controller and action name of the screen
        [Column("URL")]
        public string? Url { get; set; }
        
        [Column("IsCore")]
        public bool? IsCore { get; set; }

        [Column("Enterprise")]
        public bool? Enterprise { get; set; }
        
        [Column("Prime")]
        public bool? Prime { get; set; }
        [Column("IsAngular")]
        public bool? IsAngular { get; set; }
    }
}
