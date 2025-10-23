using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysRecordWebhookVw
    {
        [Column("ID")]
        public long Id { get; set; }

        public string? ErrorReason { get; set; }

        public string? ErrorCode { get; set; }

        public string? Data { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public bool? IsSended { get; set; }

        public string? MethodName { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsDeleted { get; set; }

        [Column("SCREEN_ID")]
        public long ScreenId { get; set; }

        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }

        [Column("SCREEN_NAME2")]
        [StringLength(50)]
        public string? ScreenName2 { get; set; }

        [Column("System_Id")]
        public int SystemId { get; set; }

        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }

        [Column("System_Name2")]
        [StringLength(50)]
        public string? SystemName2 { get; set; }

        [Column("Process_ID")]
        public long? ProcessId { get; set; }

        public string? ProcessName { get; set; }

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        public bool? IsSecurityProtocol { get; set; }

        public bool? IsSuccessCodeInBody { get; set; }

        public string? PathSuccessCode { get; set; }

        public string? SuccessCode { get; set; }

        [Column("Auth_Id")]
        public long? AuthId { get; set; }

        public bool? IsResendNewData { get; set; }

        [Column("App_Id")]
        public long? AppId { get; set; }

        [Column("App_Name")]
        [StringLength(50)]
        public string? AppName { get; set; }

        [Column("App_Name2")]
        [StringLength(50)]
        public string? AppName2 { get; set; }

        public long? CreatedBy { get; set; }

        [StringLength(50)]
        public string? CreatedName { get; set; }

        public long? ModifiedBy { get; set; }

        [StringLength(50)]
        public string? ModifiedByName { get; set; }

        public string? LinkPage { get; set; }

        [Column("Reference_ID")]
        public string? ReferenceId { get; set; }
    }
}
