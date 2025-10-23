using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logix.Domain.Main;

[Table("Sys_Zatca_Invoice_Type")]
public partial class SysZatcaInvoiceType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Name2 { get; set; }

    public int? InvoiceType { get; set; }

    [StringLength(50)]
    public string? InvoiceTypeCode { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }
}
