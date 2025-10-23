using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_CreateUser_Requst")]
    public partial class SysCreateUserRequst:TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Emp_ID")]
        public int? EmpId { get; set; }
        public string? Files { get; set; }
        public bool? Approve { get; set; }
        [Column("App_ID")]
        public int? AppId { get; set; }
        [Column("Agreement_list")]
        public string? AgreementList { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(50)]
        public string? Mobile { get; set; }
        [Column("User_Name")]
        [StringLength(500)]
        public string? UserName { get; set; }
        [StringLength(500)]
        public string? Password { get; set; }
    }
}
