using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysAnnouncementLocationVw
    {
        [Column("ID")]
        public long? Id { get; set; }
        [Column("Location_Name")]
        [StringLength(250)]
        public string? LocationName { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("Catagories_ID")]
        public int? CatagoriesId { get; set; }
        [Column("Sort_no")]
        public int? SortNo { get; set; }
    }
}
