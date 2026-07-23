namespace OnlineShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }



        // سفارش
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;




        // محصول
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;




        // اطلاعات ذخیره شده زمان خرید
        public string ProductName { get; set; } = string.Empty;


        public decimal Price { get; set; }


        public int Quantity { get; set; }

    }
}