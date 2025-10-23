using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Keyless]
public partial class InvestEmployeeVvw
{
    [Column("Emp_name")]
    [StringLength(250)]
    public string? EmpName { get; set; }

    [StringLength(250)]
    public string? Name { get; set; }

    [Column("ISDEL")]
    public bool? Isdel { get; set; }

    [Column("Emp_ID")]
    [StringLength(50)]
    public string EmpId { get; set; } = null!;

    [Column("USER_ID")]
    public long? UserId { get; set; }

    [Column("BRANCH_ID")]
    public int? BranchId { get; set; }

    [Column("Job_Type")]
    public int? JobType { get; set; }

    [Column("BRA_NAME")]
    public string? BraName { get; set; }

    [Column("Job_Catagories_ID")]
    public int? JobCatagoriesId { get; set; }

    [Column("Status_ID")]
    public int? StatusId { get; set; }

    [Column("Status_name")]
    [StringLength(250)]
    public string? StatusName { get; set; }

    [Column("Cat_name")]
    [StringLength(250)]
    public string? CatName { get; set; }

    [Column("Emp_name2")]
    [StringLength(250)]
    public string? EmpName2 { get; set; }
}
