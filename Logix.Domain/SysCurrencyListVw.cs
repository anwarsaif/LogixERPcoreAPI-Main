using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCurrencyListVw
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(101)]
        public string? Title { get; set; }
        [StringLength(101)]
        public string? Title2 { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
