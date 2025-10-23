using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysFilesDocumentVw
    {
        [Column("ID")]
        public long Id { get; set; }

        [Column("Facility_ID")]
        public int? FacilityId { get; set; }

        [Column("System_ID")]
        public long? SystemId { get; set; }

        [Column("Screen_ID")]
        public long? ScreenId { get; set; }

        [Column("File_Name")]
        [StringLength(250)]
        public string? FileName { get; set; }

        [Column("File_Type")]
        public int? FileType { get; set; }

        [Column("File_Type_Name")]
        [StringLength(250)]
        public string? FileTypeName { get; set; }

        public bool? Mandatory { get; set; }

        public long? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        public bool? IsDeleted { get; set; }

        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }

        [Column("SCREEN_NAME")]
        [StringLength(50)]
        public string? ScreenName { get; set; }

        [Column("App_Type_ID")]
        public long? AppTypeId { get; set; }
    }

}