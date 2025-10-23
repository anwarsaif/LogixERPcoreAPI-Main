using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Screen_Permission")]
    [Index("GroupId", Name = "Index_Group_ID")]
    [Index("UserId", Name = "indx_User_id")]
    [Index("ScreenId", Name = "indx_screen_id")]
    [Index("ScreenShow", Name = "indx_show")]
    public partial class SysScreenPermission
    {
        [Key]
        [Column("PRIVE_ID")]
        public long PriveId { get; set; }

        [Column("USER_ID")]
        public long? UserId { get; set; }

        [Column("SCREEN_ID")]
        public long? ScreenId { get; set; }

        [Column("SCREEN_SHOW")]
        public bool? ScreenShow { get; set; }

        [Column("SCREEN_ADD")]
        public bool? ScreenAdd { get; set; }

        [Column("SCREEN_EDIT")]
        public bool? ScreenEdit { get; set; }

        [Column("SCREEN_DELETE")]
        public bool? ScreenDelete { get; set; }

        [Column("SCREEN_PRINT")]
        public bool? ScreenPrint { get; set; }

        [Column("GroupID")]
        public int? GroupId { get; set; }

        [Column("SCREEN_EXPORT")]
        public bool? ScreenExport { get; set; }

        [Column("SCREEN_IMPORT")]
        public bool? ScreenImport { get; set; }

        [Column("SCREEN_APPROVAL")]
        public bool? ScreenApproval { get; set; }

        [Column("SCREEN_REJECT")]
        public bool? ScreenReject { get; set; }

        [Column("SCREEN_View")]
        public bool? ScreenView { get; set; }
    }
}
