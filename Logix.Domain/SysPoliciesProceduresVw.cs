using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysPoliciesProceduresVw
    {
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
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        [Column("Type_Name")]
        [StringLength(250)]
        public string? TypeName { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Type_Name2")]
        [StringLength(250)]
        public string? TypeName2 { get; set; }
        [Column("GroupsID")]
        public string? GroupsId { get; set; }
    }
}
