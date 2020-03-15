using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models
{
    public class CustomerType
    {
        [Key]
        [Column(Order = 1, TypeName = "char(3)")]
        public string Code { get; set; }

        [Required]
        [Column(Order = 2, TypeName = "nvarchar(30)")]
        public string Name { get; set; }

        [Column(Order = 3, TypeName = "nvarchar(50)")]
        public string CreatedBy { get; set; }

        [Column(Order = 4)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 5, TypeName = "nvarchar(50)")]
        public string UpdatedBy { get; set; }

        [Column(Order = 6)]
        public DateTime UpdatedDate { get; set; }
    }
}
