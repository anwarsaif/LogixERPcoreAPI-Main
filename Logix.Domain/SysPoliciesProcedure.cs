using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Policies_Procedures")]
    public partial class SysPoliciesProcedure : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Type_ID")]
        public int? TypeId { get; set; }
        [StringLength(2500)]
        public string? Name { get; set; }
        [StringLength(2500)]
        public string? Name2 { get; set; }
        [Column("File_URL")]
        public string? FileUrl { get; set; }
        
        public bool? IsActive { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("GroupsID")]
        public string? GroupsId { get; set; }
    }
}
