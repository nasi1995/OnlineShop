using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        public string Mobile { get; set; } = string.Empty;


        [Required(ErrorMessage = "استان را وارد کنید")]
        public string Province { get; set; } = string.Empty;


        [Required(ErrorMessage = "شهر را وارد کنید")]
        public string City { get; set; } = string.Empty;


        [Required(ErrorMessage = "آدرس را وارد کنید")]
        public string Address { get; set; } = string.Empty;


        [Required(ErrorMessage = "کد پستی را وارد کنید")]
        public string PostalCode { get; set; } = string.Empty;


        public string ShippingMethod { get; set; } = "پست";


        public string PaymentMethod { get; set; } = "آنلاین";
    }
}