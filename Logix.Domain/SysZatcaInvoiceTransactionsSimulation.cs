using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Sys_Zatca_Invoice_Transactions_Simulation")]
public partial class SysZatcaInvoiceTransactionsSimulation : TraceEntity
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("SAL_Transactions_ID")]
    public long? SalTransactionsId { get; set; }

    public string? InvoiceHash { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public bool? IsReportedToZatca { get; set; }

    public string? ReportingResult { get; set; }

    public string? ReportingStatus { get; set; }

    public string? QrCode { get; set; }

    public string? SignedXml { get; set; }

    [Column("Facility_ID")]
    public long? FacilityId { get; set; }

    [Column("Invoice_According_Type_ID")]
    public long? InvoiceAccordingTypeId { get; set; }

    public string? InvoiceOrder { get; set; }

    public string? SignedXmlPath { get; set; }

    [Column("Branch_Id")]
    public int? BranchId { get; set; }
}
