using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main;

[Keyless]
[Table("Sys_SMS")]
public partial class SysSms
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("Receiver_Mobile")]
    public string? ReceiverMobile { get; set; }

    public string? Message { get; set; }

    [Column("Facility_ID")]
    public long? FacilityId { get; set; }

    public long? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }
}
