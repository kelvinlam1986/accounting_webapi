using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models
{
    public class Bank
    {
        [Key]
        [Column(Order = 1, TypeName = "char(3)")]
        public string Code { get; set; }

        [Required]
        [Column(Order = 2, TypeName = "nvarchar(30)")]
        public string Name { get; set; }

        [Required]
        [Column(Order = 3, TypeName = "nvarchar(50)")]
        public string Address { get; set; }

        [Column(Order = 4, TypeName = "nvarchar(50)")]
        public string CreatedBy { get; set; }

        [Column(Order = 5)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 6, TypeName = "nvarchar(50)")]
        public string UpdatedBy { get; set; }

        [Column(Order = 7)]
        public DateTime UpdatedDate { get; set; }
    }
}
