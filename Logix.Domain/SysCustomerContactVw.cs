using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCustomerContactVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(1500)]
        public string? Name { get; set; }
        [Column("ID_NO")]
        [StringLength(50)]
        public string? IdNo { get; set; }
        [Column("Cus_Id")]
        public int? CusId { get; set; }
        [StringLength(10)]
        public string? Mobile { get; set; }
        [StringLength(50)]
        public string? Phone { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [Column("Branch_ID")]
        public int? BranchId { get; set; }
        [Column("Job_Name")]
        [StringLength(1500)]
        public string? JobName { get; set; }
        [Column("Job_Address")]
        [StringLength(2500)]
        public string? JobAddress { get; set; }
        [Column("Customer_Code")]
        [StringLength(250)]
        public string? CustomerCode { get; set; }
        [Column("Customer_Name")]
        [StringLength(2500)]
        public string? CustomerName { get; set; }
        [Column("Cus_Type_Id")]
        public int? CusTypeId { get; set; }
    }
}
