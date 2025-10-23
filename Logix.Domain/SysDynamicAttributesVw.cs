using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Keyless]
public partial class SysDynamicAttributesVw
{
    [Column("ID")]
    public long Id { get; set; }

    public Guid DynamicAttributeId { get; set; }

    [StringLength(50)]
    public string? DataTypeName { get; set; }

    public int? DataTypeId { get; set; }

    [StringLength(2000)]
    public string? AttributeName { get; set; }

    public int? SortOrder { get; set; }

    public bool? Required { get; set; }

    [Column("LookUp_Catagories_ID")]
    public int? LookUpCatagoriesId { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    [Column("Step_ID")]
    public long? StepId { get; set; }

    public int? MaxLength { get; set; }

    [Column("Default_Value")]
    public string? DefaultValue { get; set; }

    [StringLength(2000)]
    public string? AttributeName2 { get; set; }

    [Column("Table_ID")]
    public long? TableId { get; set; }

    [Column("Screen_ID")]
    public long? ScreenId { get; set; }
}
