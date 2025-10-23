using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    [Table("Sys_MailServer")]
    public partial class SysMailServer:TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Type_ID")]
        public int? TypeId { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
        [Column("Smtp_Server")]
        [StringLength(2500)]
        public string? SmtpServer { get; set; }
        [Column("Smtp_Port")]
        [StringLength(50)]
        public string? SmtpPort { get; set; }
        [Column("SSL")]
        public bool? Ssl { get; set; }
        [Column("TLS")]
        public bool? Tls { get; set; }
        [StringLength(50)]
        public string? Username { get; set; }
        [StringLength(50)]
        public string? Password { get; set; }
        [Column("User_ID")]
        public long? UserId { get; set; }
     
    }
}
