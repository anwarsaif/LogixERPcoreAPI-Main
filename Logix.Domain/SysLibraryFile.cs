using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Library_Files")]
    public partial class SysLibraryFile:TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Refrance_Code")]
        [StringLength(50)]
        public string? RefranceCode { get; set; }
        [Column("File_Name")]
        [StringLength(50)]
        public string? FileName { get; set; }
        [Column("File_Description")]
        [StringLength(4000)]
        public string? FileDescription { get; set; }
        [Column("File_Date")]
        [StringLength(10)]
        public string? FileDate { get; set; }
        [Column("File_Type")]
        public int? FileType { get; set; }
        [Column("Source_File")]
        [StringLength(500)]
        public string? SourceFile { get; set; }
        [Column("File_URL")]
        public string? FileUrl { get; set; }
        [Column("File_Ext")]
        [StringLength(50)]
        public string? FileExt { get; set; }

        [Column("End_Date_File")]
        [StringLength(10)]
        public string? EndDateFile { get; set; }
        [Column("Project_ID")]
        public long? ProjectId { get; set; }
    }
}
