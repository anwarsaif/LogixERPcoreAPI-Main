using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Table("ZatcaVATCategoriesReasons")]
public partial class ZatcaVatcategoriesReason : TraceEntity
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    public string CategoryName { get; set; } = null!;

    [StringLength(1)]
    public string CategoryCode { get; set; } = null!;

    [Column("Sys_VAT_Group_ID")]
    public int? SysVatGroupId { get; set; }

    [StringLength(255)]
    public string? ReasonArabic { get; set; }

    [StringLength(255)]
    public string? ReasonEnglish { get; set; }
}
