using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{

    [Table("Sys_Package")]
    public partial class SysPackage
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("Package_ID")]
        public long? PackageId { get; set; }
    }
}