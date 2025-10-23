using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Fav_Menu")]
    public partial class SysFavMenu
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        [Column("User_ID")]
        public long? UserId { get; set; }
        [Column("Sort_No")]
        public int? SortNo { get; set; }
    }
}
