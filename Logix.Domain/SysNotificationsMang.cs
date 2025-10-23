using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Notifications_Mang")]
public partial class SysNotificationsMang : TraceEntity
{
    [Key]
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

    [Column("Condition_Others")]
    [StringLength(2000)]
    public string? ConditionOthers { get; set; }

    [Column("Ahead_Of")]
    public long? AheadOf { get; set; }

    [Column("Assignee_Type_ID")]
    public int? AssigneeTypeId { get; set; }

    [Column("Group_ID")]
    [StringLength(250)]
    public string? GroupId { get; set; }
}
