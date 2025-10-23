using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    [Table("Sys_Customer_Branch")]
    public partial class SysCustomerBranch
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Cus_Id")]
        public int? CusId { get; set; }
        [StringLength(1500)]
        public string? Name { get; set; }
        [Column("ID_NO")]
        [StringLength(50)]
        public string? IdNo { get; set; }
        [Column("Job_Name")]
        [StringLength(1500)]
        public string? JobName { get; set; }
        [Column("Job_Address")]
        [StringLength(2500)]
        public string? JobAddress { get; set; }
        [StringLength(10)]
        public string? Mobile { get; set; }
        [StringLength(50)]
        public string? Phone { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(50)]
        public string? Email2 { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
    }
}
