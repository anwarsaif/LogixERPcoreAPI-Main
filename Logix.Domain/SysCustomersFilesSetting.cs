using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main;

[Table("Sys_Customers_Files_Settings")]
public partial class SysCustomersFilesSetting : TraceEntity
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CustomerTypeID")]
    public int? CustomerTypeId { get; set; }

    public int? FileTypeId { get; set; }

    public bool? IsRequired { get; set; }

    public bool? RequiresAuthorization { get; set; }

    [Column("Facility_ID")]
    public int? FacilityId { get; set; }
}
