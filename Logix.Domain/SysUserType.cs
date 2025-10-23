using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("SYS_USER_TYPE")]
public partial class SysUserType
{
    [Key]
    [Column("USER_TYPE_ID")]
    public int UserTypeId { get; set; }

    [Column("USER_TYPE_NAME")]
    [StringLength(50)]
    public string? UserTypeName { get; set; }
}
