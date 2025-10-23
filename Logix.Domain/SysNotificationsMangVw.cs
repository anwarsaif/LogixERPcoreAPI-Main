using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Keyless]
public partial class SysNotificationsMangVw
{
    [Column("ID")]
    public long Id { get; set; }
    [StringLength(500)]
    public string? Name { get; set; }
    [Column("User_ID")]
    [StringLength(250)]
    public string? UserId { get; set; }
    [Column("Table_ID")]
    public long? TableId { get; set; }
    [Column("Select_Field_ID")]
    public long? SelectFieldId { get; set; }
    [Column("Condition_Field_ID")]
    public long? ConditionFieldId { get; set; }
    [Column("Ahead_Of")]
    public long? AheadOf { get; set; }
    [Column("Condition_Others")]
    [StringLength(2000)]
    public string? ConditionOthers { get; set; }
    public bool? IsDeleted { get; set; }
    [Column("Table_Description")]
    [StringLength(200)]
    public string? TableDescription { get; set; }
    [Column("Desc_1")]
    [StringLength(50)]
    public string? Desc1 { get; set; }
    [Column("Table_Name")]
    [StringLength(50)]
    public string? TableName { get; set; }
    [Column("Select_Field_Name")]
    [StringLength(50)]
    public string? SelectFieldName { get; set; }
    [Column("Condition_Field_Name")]
    [StringLength(50)]
    public string? ConditionFieldName { get; set; }
    [Column("Desc_12")]
    [StringLength(50)]
    public string? Desc12 { get; set; }
    [Column("Assignee_Type_ID")]
    public int? AssigneeTypeId { get; set; }
    [Column("Group_ID")]
    [StringLength(250)]
    public string? GroupId { get; set; }
}
