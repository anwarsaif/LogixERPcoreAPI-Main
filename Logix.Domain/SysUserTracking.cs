using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_User_Tracking")]
    public partial class SysUserTracking
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        public string? Url { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("User_ID")]
        public int? UserId { get; set; }
        [Column("Emp_ID")]
        public int? EmpId { get; set; }
    }
}
