using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logix.Domain.Main
{
    [Table("Sys_System")]
    public partial class SysSystem
    {
        [Key]
        [Column("System_Id")]
        public int SystemId { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [Column("System_Name2")]
        [StringLength(50)]
        public string? SystemName2 { get; set; }
        [StringLength(50)]
        public string? Folder { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("Sys_Sort")]
        public int? SysSort { get; set; }
        [Column("Desc_1")]
        [StringLength(2500)]
        public string? Desc1 { get; set; }
        [Column("Desc_2")]
        [StringLength(2500)]
        public string? Desc2 { get; set; }
        [Column("Icon_Css")]
        [StringLength(50)]
        public string? IconCss { get; set; }
        [Column("Show_In_Home")]
        public bool? ShowInHome { get; set; }
        [Column("Color_Css")]
        [StringLength(2500)]
        public string? ColorCss { get; set; }
        [Column("Default_Page")]
        [StringLength(2500)]
        public string? DefaultPage { get; set; }
        [Column("Short_Name")]
        [StringLength(50)]
        public string? ShortName { get; set; }
        [Column("Short_Name2")]
        [StringLength(50)]
        public string? ShortName2 { get; set; }

        //============== added by najmadeen for area, controller and view name 
        [StringLength(50)]
        public string? Controller { get; set; }


        [StringLength(50)]
        public string? Action { get; set; }

        [StringLength(50)]
        public string? Area { get; set; }

        [Column("IsCore")]
        public bool IsCore { get; set; }
        [Column("IsAngular")]
        public bool? IsAngular { get; set; }
    }
}
