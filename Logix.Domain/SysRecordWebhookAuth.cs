using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Record_Webhook_Auth")]
    public partial class SysRecordWebhookAuth : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("WebHook_Auth_ID")]
        public long? WebHookAuthId { get; set; }

        public string? ErrorReason { get; set; }

        public string? ErrorCode { get; set; }

        public string? Data { get; set; }

        public bool? IsSended { get; set; }
    }
}
