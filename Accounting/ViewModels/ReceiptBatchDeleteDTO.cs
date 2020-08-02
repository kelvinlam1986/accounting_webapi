using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class ReceiptBatchDeleteDTO
    {
        [Required(ErrorMessage = "Số lô phiếu thu cần được cung cấp.")]
        public string ReceiptBatchNo { get; set; }
    }
}
