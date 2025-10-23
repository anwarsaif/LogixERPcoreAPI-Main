using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main;

[Table("Sys_DynamicAttributes")]
public partial class SysDynamicAttribute : TraceEntity
{
    [Column("ID")]
    public long Id { get; set; }

    [Key]
    public Guid DynamicAttributeId { get; set; }

    [Column("Screen_ID")]
    public long? ScreenId { get; set; }

    public int? DataTypeId { get; set; }

    [StringLength(2000)]
    public string? AttributeName { get; set; }

    public int? SortOrder { get; set; }

    public bool? Required { get; set; }

    [Column("LookUp_Catagories_ID")]
    public int? LookUpCatagoriesId { get; set; }


    [Column("Step_ID")]
    public long? StepId { get; set; }

    [Column("Default_Value")]
    public string? DefaultValue { get; set; }

    public int? MaxLength { get; set; }

    [Column("Table_ID")]
    public long? TableId { get; set; }

    [StringLength(2000)]
    public string? AttributeName2 { get; set; }
}
