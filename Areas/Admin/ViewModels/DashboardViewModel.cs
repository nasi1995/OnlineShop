using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public int ProductsCount { get; set; }

        public int CategoriesCount { get; set; }

        public int UsersCount { get; set; }

        public int OrdersCount { get; set; }

        public decimal TotalSales { get; set; }

        public List<Order> LatestOrders { get; set; } = [];

        public List<ApplicationUser> LatestUsers { get; set; } = [];
    }
}
