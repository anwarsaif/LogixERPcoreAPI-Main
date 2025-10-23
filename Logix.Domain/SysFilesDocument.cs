using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Files_Document")]
    public partial class SysFilesDocument : TraceEntity
    {
        [Key]
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

        public bool? Mandatory { get; set; }

        [Column("App_Type_ID")]
        public long? AppTypeId { get; set; }
    }
}