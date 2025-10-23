using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{

    [Table("Sys_WebHook")]
    public partial class SysWebHook : TraceEntity
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

        [Column("Screen_ID")]
        public long? ScreenId { get; set; }

        [Column("Process_ID")]
        public long? ProcessId { get; set; }

        public bool? IsEnabled { get; set; }

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        public string? QueryDetails { get; set; }

        public string? BodyDetails { get; set; }

        public bool? IsSecurityProtocol { get; set; }

        public bool? IsSuccessCodeInBody { get; set; }

        public string? PathSuccessCode { get; set; }

        public string? SuccessCode { get; set; }

        public string? LinkPage { get; set; }

        [Column("Auth_Id")]
        public long? AuthId { get; set; }

        [Column("App_Id")]
        public long? AppId { get; set; }

        public bool? IsResendNewData { get; set; }
    }

}