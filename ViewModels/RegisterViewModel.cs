using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید.")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل را وارد کنید.")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر وارد کنید.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور را وارد کنید.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید.")]
        [Compare("Password", ErrorMessage = "رمزهای عبور یکسان نیستند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}