using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysFormsVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(50)]
        public string? Name1 { get; set; }
        [Column("NAme2")]
        [StringLength(50)]
        public string? Name2 { get; set; }
        [StringLength(150)]
        public string? Url { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [Column("System_ID")]
        public long? SystemId { get; set; }

        //[Column("System_Name2")]
        //[StringLength(50)]
        //public string? SystemName2 { get; set; }
    }
}
