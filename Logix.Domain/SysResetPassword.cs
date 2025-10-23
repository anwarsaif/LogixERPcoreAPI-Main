using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_ResetPassword")]
    public partial class SysResetPassword
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("User_ID")]
        public int? UserId { get; set; }
        [Column("D_Date", TypeName = "datetime")]
        public DateTime? DDate { get; set; }
        [Column("Is_Update")]
        public bool? IsUpdate { get; set; }
        [StringLength(500)]
        public string? Email { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public byte VerificationType { get; set; }
        [StringLength(15)]
        public string? MobileNumber { get; set; }
        [StringLength(10)]
        public string VerificationCode { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime ExpiryTime { get; set; }
    }
}
