using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Customer_Group_Accounts")]
    public partial class SysCustomerGroupAccount : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Group_ID")]
        public long? GroupId { get; set; }
        [Column("Reference_Type_ID")]
        public int? ReferenceTypeId { get; set; }
        [Column("Account_ID")]
        public long? AccountId { get; set; }
    }
}
