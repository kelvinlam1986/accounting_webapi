using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class CustomerTypeUpdateDTO
    {
        [Required(ErrorMessage = "Mã loại khách hàng cần được cung cấp.")]
        [StringLength(3, ErrorMessage = "Mã loại khách hàng tối đa 3 ký tự")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Tên loại khách hàng cần được cung cấp.")]
        public string Name { get; set; }
    }
}
