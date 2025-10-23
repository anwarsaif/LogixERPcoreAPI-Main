using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCustomerBranchVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(1500)]
        public string? Name { get; set; }
        [Column("Cus_Id")]
        public int? CusId { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Cus_Name")]
        [StringLength(50)]
        public string? CusName { get; set; }
    }
}
