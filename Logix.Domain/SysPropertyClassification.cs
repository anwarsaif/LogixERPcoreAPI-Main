using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main;

[Table("Sys_Property_Classifications")]
public partial class SysPropertyClassification : TraceEntity
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? Name2 { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    //[Column(TypeName = "datetime")]
    //public DateTime? CreatedOn { get; set; }
    //public long? CreatedBy { get; set; }
    //[Column(TypeName = "datetime")]
    //public DateTime? ModifiedOn { get; set; }
    //public long? ModifiedBy { get; set; }
    //public bool? IsDeleted { get; set; }
}
