using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Order
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "لطفاً نام و نام خانوادگی را وارد کنید")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً شماره موبایل را وارد کنید")]
        public string Mobile { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً استان را انتخاب کنید")]
        public string Province { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً شهر را انتخاب کنید")]
        public string City { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً آدرس را وارد کنید")]
        public string Address { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً کد پستی را وارد کنید")]
        public string PostalCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً روش ارسال را انتخاب کنید")]
        public string ShippingMethod { get; set; } = string.Empty;


        [Required(ErrorMessage = "لطفاً روش پرداخت را انتخاب کنید")]
        public string PaymentMethod { get; set; } = string.Empty;


        public decimal TotalPrice { get; set; }


        public string PaymentStatus { get; set; } = "Pending";


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "وضعیت سفارش")]
        public string OrderStatus { get; set; } = "در انتظار بررسی";

        // کاربر ثبت کننده سفارش
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }


        // آیتم‌های سفارش
        public virtual ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();
    }
}