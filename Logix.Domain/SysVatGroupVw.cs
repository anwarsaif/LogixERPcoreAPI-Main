using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public class SysVatGroupVw
    {
        [Column("VAT_ID")]
        public long VatId { get; set; }
        [Column("VAT_Name")]
        [StringLength(10)]
        public string? VatName { get; set; }
        [Column("VAT_Rate", TypeName = "decimal(18, 2)")]
        public decimal? VatRate { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Sales_VAT_Account_ID")]
        public long? SalesVatAccountId { get; set; }
        [Column("Sales_VAT_Account_Code")]
        [StringLength(50)]
        public string? SalesVatAccountCode { get; set; }
        [Column("Sales_VAT_Account_Name")]
        [StringLength(255)]
        public string? SalesVatAccountName { get; set; }
        [Column("Purchases_VAT_Account_ID")]
        public long? PurchasesVatAccountId { get; set; }
        [Column("Purchases_VAT_Account_Code")]
        [StringLength(50)]
        public string? PurchasesVatAccountCode { get; set; }
        [Column("Purchases_VAT_Account_Name")]
        [StringLength(255)]
        public string? PurchasesVatAccountName { get; set; }
    }
}
