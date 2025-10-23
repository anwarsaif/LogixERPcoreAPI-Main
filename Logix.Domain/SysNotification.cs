using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Notifications")]
    public partial class SysNotification
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("User_ID")]
        public long? UserId { get; set; }
        [Column("Msg_Txt")]
        public string? MsgTxt { get; set; }
        [Column("Is_Read")]
        public bool? IsRead { get; set; }
        [Column("Read_Date", TypeName = "datetime")]
        public DateTime? ReadDate { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("URL")]
        public string? Url { get; set; }
        [Column("ActivityLogID")]
        public long? ActivityLogId { get; set; }
        [Column("Table_ID")]
        public long? TableId { get; set; }
    }
}
