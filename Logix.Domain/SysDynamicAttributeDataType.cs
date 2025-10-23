using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_DynamicAttributeDataTypes")]
public partial class SysDynamicAttributeDataType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? DataTypeCaption { get; set; }

    [StringLength(50)]
    public string? DataTypeName { get; set; }
}
