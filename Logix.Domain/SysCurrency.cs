using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Currency")]
    public partial class SysCurrency : TraceEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Name2 { get; set; }
        [Column("Subunit_Name")]
        [StringLength(50)]
        public string? SubunitName { get; set; }
        [Column("Subunit_Name2")]
        [StringLength(50)]
        public string? SubunitName2 { get; set; }
        [Column("Decimal_point")]
        public int? DecimalPoint { get; set; }
        [StringLength(50)]
        public string? Symbol { get; set; }
    }
}
