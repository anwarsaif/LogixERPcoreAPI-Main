using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Exchange_Rate")]
    public class SysExchangeRate : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Currency_From_ID")]
        public long? CurrencyFromID { get; set; }
        [Column("Currency_To_ID")]
        public long? CurrencyToID { get; set; }
        [Column("Exchange_Date")]
        public string? ExchangeDate { get; set; }
        [Column("Exchange_Rate")]
        public decimal? ExchangeRate { get; set; }

        //public long? CreatedBy { get; set; }
        //[Column(TypeName = "datetime")]
        //public DateTime? CreatedOn { get; set; }
        //public long? ModifiedBy { get; set; }
        //[Column(TypeName = "datetime")]
        //public DateTime? ModifiedOn { get; set; }
        //public bool? IsDeleted { get; set; }
    }
}
