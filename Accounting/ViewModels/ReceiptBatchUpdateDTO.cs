using System;
using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class ReceiptBatchUpdateDTO
    {
        [Required(ErrorMessage = "Số lô phiếu thu cần được cung cấp.")]
        [StringLength(10, ErrorMessage = "Số lô phiếu thu tối đa 10 ký tự")]
        public string ReceiptBatchNo { get; set; }
        [Required(ErrorMessage = "Ngày nhập lô phiếu thu cần được cung cấp")]
        public DateTime ReceiptBatchDate { get; set; }
        public string DescriptionInVietNamese { get; set; }
        [Required(ErrorMessage = "Trạng thái lô cần được cung cấp")]
        public bool BatchStatus { get; set; }
    }
}
