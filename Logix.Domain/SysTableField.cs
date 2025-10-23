using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Table_Field")]
public partial class SysTableField
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("Desc_1")]
    [StringLength(50)]
    public string? Desc1 { get; set; }

    [Column("Table_ID")]
    public long? TableId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [Column("Desc_2")]
    [StringLength(50)]
    public string? Desc2 { get; set; }
}
