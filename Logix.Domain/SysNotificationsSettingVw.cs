using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysNotificationsSettingVw
    {
        [Column("ID")]
        public long Id { get; set; }
        public string? Users { get; set; }
        [Column("Msg_Txt")]
        public string? MsgTxt { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Users_Name")]
        public string? UsersName { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [Column("ActionType_ID")]
        public int? ActionTypeId { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
        [Column("Action_TypeName")]
        [StringLength(250)]
        public string? ActionTypeName { get; set; }
    }
}
