using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Customer_Type")]
    public partial class SysCustomerType
    {
        [Key]
        [Column("Type_ID")]
        public int TypeId { get; set; }
        [Column("Cus_Type_Name")]
        [StringLength(50)]
        public string? CusTypeName { get; set; }
        [Column("Pervix_Code")]
        [StringLength(50)]
        public string? PervixCode { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
    }
}
