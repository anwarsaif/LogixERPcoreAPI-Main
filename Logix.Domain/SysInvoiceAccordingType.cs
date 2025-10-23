using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Invoice_According_Type")]
public partial class SysInvoiceAccordingType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Name2 { get; set; }

    [Column("InvoiceAccordingTypeID")]
    public long? InvoiceAccordingTypeId { get; set; }

    [Column("System_ID")]
    public long? SystemId { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }
}
