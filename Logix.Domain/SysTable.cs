using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Table")]
public partial class SysTable
{
    [Key]
    [Column("Table_ID")]
    public int TableId { get; set; }

    [Column("Table_Description")]
    [StringLength(200)]
    public string? TableDescription { get; set; }

    [StringLength(50)]
    public string? Primarykey { get; set; }

    [Column("Table_Name")]
    [StringLength(50)]
    public string? TableName { get; set; }

    public string? Condition { get; set; }

    [Column("Screen_URL")]
    public string? ScreenUrl { get; set; }

    [Column("System_ID")]
    [StringLength(100)]
    public string? SystemId { get; set; }
}
