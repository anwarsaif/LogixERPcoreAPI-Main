using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Screen_Permission_Properties")]
    public partial class SysScreenPermissionProperty : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Property_ID")]
        public long? PropertyId { get; set; }
        public bool? Allow { get; set; }
        public string? Value { get; set; }
        [Column("User_ID")]
        public long? UserId { get; set; }
    }
}
