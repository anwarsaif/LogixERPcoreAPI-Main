using Logix.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Logix.Domain.Main
{
    [Table("CRM_EmailTemplateAttach")]
    public partial class CrmEmailTemplateAttach :TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [Column("File_Url")]
        public string? FileUrl { get; set; }
        [Column("Template_ID")]
        public long? TemplateId { get; set; }
       
    }
}
