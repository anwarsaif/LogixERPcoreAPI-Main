using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysScreenPermissionVw
    {
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
        [Column("System_Id")]
        public int? SystemId { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
    }
}
