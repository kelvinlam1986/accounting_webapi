using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models
{
    public class ReceiptBatch
    {
        [Key]
        [Column(Order = 1, TypeName = "varchar(10)")]
        public string ReceiptBatchNo { get; set; }

        [Required]
        [Column(Order = 2)]
        public DateTime ReceiptBatchDate { get; set; }

        [Column(Order = 3, TypeName = "nvarchar(150)")]
        public string DescriptionInVietNamese { get; set; }

        [Column(Order = 4, TypeName = "varchar(10)")]
        public bool BatchStatus { get; set; }

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
