using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix.Domain.Base;

namespace Logix.Domain.Main
{
    [Table("Sys_Cites")]
    public class SysCites : TraceEntity
    {
        [Key]
        [Column("City_ID")]
        public long CityID { get; set; }
        [Column("City_Name")]
        public string? CityName { get; set; }
        [Column("Code")]
        [StringLength(50)]
        public string? Code { get; set; }
        [Column("Country_ID")]
        public int? CountryID { get; set; }
        [Column("Parent_ID")]
        public long? ParentID { get; set; }
        [Column("TypeID")]
        public long? TypeID { get; set; }
        [Column("City_Name2")]
        [StringLength(50)]
        public string? CityName2 { get; set; }
    }
}
