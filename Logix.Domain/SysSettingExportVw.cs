using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysSettingExportVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Facility_ID")]
        public int? FacilityId { get; set; }
        [Column("System_ID")]
        public long? SystemId { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        public string? Query { get; set; }
        public int? Type { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string? Name2 { get; set; }
    }
}
