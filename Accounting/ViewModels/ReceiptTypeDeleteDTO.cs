using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class ReceiptTypeDeleteDTO
    {
        [Required(ErrorMessage = "Loại phiếu thu cần được cung cấp.")]
        public string Code { get; set; }
    }
}
