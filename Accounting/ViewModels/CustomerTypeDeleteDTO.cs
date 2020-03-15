using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class CustomerTypeDeleteDTO
    {
        [Required(ErrorMessage = "Loại khách hàng cần được cung cấp.")]
        public string Code { get; set; }
    }
}
