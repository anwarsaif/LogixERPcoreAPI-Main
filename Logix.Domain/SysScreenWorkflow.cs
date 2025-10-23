using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Domain.Main
{
    [Table("Sys_Screen_Workflow")]
    public partial class SysScreenWorkflow
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        public string? Description { get; set; }
        [Column("Workflow_ID")]
        public long? WorkflowId { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
