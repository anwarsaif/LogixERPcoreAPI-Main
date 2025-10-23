using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    [Table("Sys_Country")]
    public partial class SysCountry
    {
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Name2 { get; set; }
        [StringLength(2)]
        public string? TwoLetterIsoCode { get; set; }
        [StringLength(3)]
        public string? ThreeLetterIsoCode { get; set; }
        public int? NumericIsoCode { get; set; }
        public int? DisplayOrder { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
