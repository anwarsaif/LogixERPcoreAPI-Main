using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Record_Webhook")]
    public partial class SysRecordWebhook
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("WebHookID")]
        public long? WebHookId { get; set; }

        public string? ErrorReason { get; set; }

        public string? ErrorCode { get; set; }

        public string? Data { get; set; }

        public long? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsSended { get; set; }

        [Column("Reference_ID")]
        public string? ReferenceId { get; set; }

        public string? LinkPage { get; set; }
    }
}
