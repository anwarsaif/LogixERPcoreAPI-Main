using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_VAT_Group")]
    public class SysVatGroup
    {
        [Key]
        [Column("VAT_ID")]
        public long VatId { get; set; }
        [Column("VAT_Name")]
        [StringLength(50)]
        public string? VatName { get; set; }
        [Column("VAT_Rate", TypeName = "decimal(18, 2)")]
        public decimal? VatRate { get; set; }
        [Column("Sales_VAT_Account_ID")]
        public long? SalesVatAccountId { get; set; }
        [Column("Purchases_VAT_Account_ID")]
        public long? PurchasesVatAccountId { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
