using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "رمز عبور فعلی را وارد کنید")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور جدید را وارد کنید")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "رمز عبور حداقل باید ۶ کاراکتر باشد")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور صحیح نیست")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}