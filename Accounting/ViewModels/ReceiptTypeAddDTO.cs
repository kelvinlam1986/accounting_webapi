using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class ReceiptTypeAddDTO
    {
        [Required(ErrorMessage = "Mã loại phiếu thu cần được cung cấp.")]
        [StringLength(3, ErrorMessage = "Mã loại phiếu thu tối đa 3 ký tự")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Tên loại phiếu thu cần được cung cấp.")]
        public string ReceiptTypeInVietnamese { get; set; }

        public string ReceiptTypeInSecondLanguage { get; set; }

        public bool ShowReceiptTypeInVietNamese { get; set; }
    }
}
