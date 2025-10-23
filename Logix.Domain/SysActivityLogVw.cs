using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysActivityLogVw
    {
        [StringLength(50)]
        public string? ActivityType { get; set; }
        [Column("ActivityLogID")]
        public long ActivityLogId { get; set; }
        [StringLength(50)]
        public string? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ActivityDate { get; set; }
        [Column("TableID")]
        public int? TableId { get; set; }
        [Column("Table_Primarykey")]
        public long? TablePrimarykey { get; set; }
        [Column("Table_Name")]
        [StringLength(50)]
        public string? TableName { get; set; }
        [Column("Table_Description")]
        [StringLength(200)]
        public string? TableDescription { get; set; }
        [Column("USER_FULLNAME")]
        [StringLength(50)]
        public string? UserFullname { get; set; }
        [Column("ActivityTypeID")]
        public int? ActivityTypeId { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
    }
}
