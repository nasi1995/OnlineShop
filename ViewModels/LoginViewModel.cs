using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور را وارد کنید.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}