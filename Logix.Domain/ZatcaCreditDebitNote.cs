using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("Zatca_CreditDebitNotes")]
public partial class ZatcaCreditDebitNote : TraceEntity
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Name2 { get; set; }

    public long Code { get; set; }
}
