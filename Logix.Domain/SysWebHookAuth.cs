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

    [Table("Sys_WebHook_Auth")]
    public partial class SysWebHookAuth : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        [Column("URl")]
        public string? Url { get; set; }

        public int? MethodType { get; set; }

        public string? Header { get; set; }

        public string? Parameter { get; set; }

        public string? Body { get; set; }

        public string? Query { get; set; }

        public bool? IsEnabled { get; set; }

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        public string? QueryDetails { get; set; }

        public string? BodyDetails { get; set; }

        public bool? IsSecurityProtocol { get; set; }

        public bool? IsSuccessCodeInBody { get; set; }

        public string? PathSuccessCode { get; set; }

        public string? SuccessCode { get; set; }

        public string? QueryAfterResult { get; set; }

        [Column("App_ID")]
        public long? AppId { get; set; }
    }
}
