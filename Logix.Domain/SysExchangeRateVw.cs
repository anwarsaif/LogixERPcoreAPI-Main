using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysExchangeRateVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Currency_From_ID")]
        public long? CurrencyFromId { get; set; }
        [Column("Currency_To_ID")]
        public long? CurrencyToId { get; set; }
        [Column("Name_From")]
        [StringLength(103)]
        public string? NameFrom { get; set; }
        [Column("Name_To")]
        [StringLength(103)]
        public string? NameTo { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Exchange_Date")]
        [StringLength(10)]
        public string? ExchangeDate { get; set; }
        [Column("Exchange_Rate", TypeName = "decimal(18, 10)")]
        public decimal? ExchangeRate { get; set; }
    }
}
