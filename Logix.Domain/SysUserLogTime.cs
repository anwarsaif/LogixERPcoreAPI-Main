using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("SYS_USER_LogTime")]
    public partial class SysUserLogTime
    {
        [Key]
        [Column("LogTime_ID")]
        public long LogTimeId { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        [Column("Login_Time", TypeName = "datetime")]
        public DateTime? LoginTime { get; set; }
        [Column("Logout_Time", TypeName = "datetime")]
        public DateTime? LogoutTime { get; set; }
        [Column("offline")]
        public bool? Offline { get; set; }
        [Column("IP_Address")]
        [StringLength(250)]
        public string? IpAddress { get; set; } 
     
    }
}
