using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models
{
    public class ReceiptType
    {
        [Key]
        [Column(Order = 1, TypeName = "char(3)")]
        public string Code { get; set; }

        [Required]
        [Column(Order = 2, TypeName = "nvarchar(50)")]
        public string ReceiptTypeInVietnamese { get; set; }

        [Column(Order = 3, TypeName = "nvarchar(50)")]
        public string ReceiptTypeInSecondLanguage { get; set; }

        [Column(Order = 4)]
        public bool ShowReceiptTypeInVietNamese { get; set; }

        [Column(Order = 5, TypeName = "nvarchar(50)")]
        public string CreatedBy { get; set; }

        [Column(Order = 6)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 7, TypeName = "nvarchar(50)")]
        public string UpdatedBy { get; set; }

        [Column(Order = 8)]
        public DateTime UpdatedDate { get; set; }
    }
}
