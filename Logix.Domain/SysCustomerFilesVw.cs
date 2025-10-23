using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysCustomerFilesVw
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("Project_Code")]
        [StringLength(50)]
        public string? ProjectCode { get; set; }
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
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("File_Type_Name")]
        [StringLength(250)]
        public string? FileTypeName { get; set; }
        [Column("File_URL")]
        public string? FileUrl { get; set; }
        [Column("File_Ext")]
        [StringLength(50)]
        public string? FileExt { get; set; }
        [Column("Customer_ID")]
        public long? CustomerId { get; set; }
        [Column("Project_Name")]
        [StringLength(2500)]
        public string? ProjectName { get; set; }
        [Column("Customer_Name")]
        [StringLength(2500)]
        public string? CustomerName { get; set; }
        [Column("USER_FULLNAME")]
        [StringLength(50)]
        public string? UserFullname { get; set; }
    }
}
