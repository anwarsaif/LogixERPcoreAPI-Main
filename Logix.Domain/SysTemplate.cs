using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Template")]
    public partial class SysTemplate : TraceEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("NAME")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Column("DETAILES", TypeName = "ntext")]
        public string? Detailes { get; set; }
        [Column("SCREEN_ID")]
        public long? ScreenId { get; set; }
        [Column("Type_ID")]
        public int? TypeId { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        public string? Url { get; set; }
    }
}
