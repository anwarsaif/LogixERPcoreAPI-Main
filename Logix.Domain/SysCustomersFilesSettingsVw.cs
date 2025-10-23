using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Keyless]
public partial class SysCustomersFilesSettingsVw
{
    [Column("ID")]
    public long Id { get; set; }

    public int? FileTypeId { get; set; }

    [StringLength(250)]
    public string? FileTypeName { get; set; }

    [Column("CustomerTypeID")]
    public int? CustomerTypeId { get; set; }

    [StringLength(50)]
    public string? CustomerTypeName { get; set; }

    public bool? IsRequired { get; set; }

    public bool? RequiresAuthorization { get; set; }

    [Column("Facility_ID")]
    public int? FacilityId { get; set; }

    public bool? IsDeleted { get; set; }
}
