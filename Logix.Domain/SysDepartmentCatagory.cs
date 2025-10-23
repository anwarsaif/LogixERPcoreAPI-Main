using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Department_Catagories")]
    public partial class SysDepartmentCatagory
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Cat_Name")]
        [StringLength(2500)]
        public string? CatName { get; set; }
    }
}
