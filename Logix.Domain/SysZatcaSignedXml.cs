using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Zatca_SignedXml")]
public partial class SysZatcaSignedXml : TraceEntity
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("Invoice_ID")]
    public long InvoiceId { get; set; }

    [Column("Invoice_According_Type_ID")]
    [StringLength(10)]
    public string InvoiceAccordingTypeId { get; set; } = null!;

    public string SignedXml { get; set; } = null!;

    [Column("Facility_ID")]
    public long FacilityId { get; set; }

    [Column("Branch_Id")]
    public int? BranchId { get; set; }
}
