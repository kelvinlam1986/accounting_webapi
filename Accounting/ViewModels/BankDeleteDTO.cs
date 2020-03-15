using System.ComponentModel.DataAnnotations;

namespace Accounting.ViewModels
{
    public class BankDeleteDTO
    {
        [Required(ErrorMessage = "Mã ngân hàng cần được cung cấp.")]
        public string Code { get; set; }
    }
}
