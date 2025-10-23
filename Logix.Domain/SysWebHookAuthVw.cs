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
    public partial class SysWebHookAuthVw
    {
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

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        public long? CreatedBy { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsEnabled { get; set; }

        public string? MethodName { get; set; }

        public string? QueryDetails { get; set; }

        public string? BodyDetails { get; set; }

        public bool? IsSecurityProtocol { get; set; }

        public bool? IsSuccessCodeInBody { get; set; }

        public string? PathSuccessCode { get; set; }

        public string? SuccessCode { get; set; }

        public string? QueryAfterResult { get; set; }

        [Column("App_ID")]
        public long? AppId { get; set; }

        [StringLength(50)]
        public string? AppName { get; set; }

        [StringLength(50)]
        public string? AppName2 { get; set; }
    }
}
