using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Notifications_Setting")]
    public partial class SysNotificationsSetting:TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [Column("ActionType_ID")]
        public int? ActionTypeId { get; set; }
        public string? Users { get; set; }
        [Column("Msg_Txt")]
        public string? MsgTxt { get; set; }
    }
}
