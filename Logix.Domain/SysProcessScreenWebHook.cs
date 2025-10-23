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
    [Table("Sys_Process_Screen_WebHook")]
    public partial class SysProcessScreenWebHook : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        public string? Name { get; set; }
    }
}
