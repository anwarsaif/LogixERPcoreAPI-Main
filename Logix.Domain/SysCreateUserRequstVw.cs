using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCreateUserRequstVw
    {
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("App_ID")]
        public int? AppId { get; set; }
        public bool? Approve { get; set; }
        [StringLength(50)]
        public string? Files { get; set; }
        [Column("Emp_ID")]
        public int? EmpId { get; set; }
        [Column("ID")]
        public long Id { get; set; }
        [Column("Emp_name")]
        [StringLength(250)]
        public string? EmpName { get; set; }
        [Column("Emp_name2")]
        [StringLength(250)]
        public string? EmpName2 { get; set; }
        [Column("ID_No")]
        [StringLength(50)]
        public string? IdNo { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string? BirthDate { get; set; }
        [Column("Birth_Place")]
        [StringLength(500)]
        public string? BirthPlace { get; set; }
        [Column("Emp_Code")]
        [StringLength(50)]
        public string EmpCode { get; set; } = null!;
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
