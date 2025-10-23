using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysRecordWebhookAuthVw
    {
        [Column("ID")]
        public long Id { get; set; }

        public string? ErrorReason { get; set; }

        public string? ErrorCode { get; set; }

        public string? Data { get; set; }

        public bool? IsSended { get; set; }

        public bool? IsDeleted { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        public bool? IsEnabled { get; set; }

        public string? MethodName { get; set; }

        public bool? IsSecurityProtocol { get; set; }

        public bool? IsSuccessCodeInBody { get; set; }

        public string? PathSuccessCode { get; set; }

        public string? SuccessCode { get; set; }

        [Column("App_ID")]
        public long? AppId { get; set; }

        [StringLength(50)]
        public string? AppName { get; set; }

        [StringLength(50)]
        public string? AppName2 { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
    }
}