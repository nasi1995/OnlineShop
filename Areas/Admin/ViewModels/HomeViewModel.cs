namespace OnlineShop.Models.ViewModels;

public class HomeViewModel
{
    public SiteSetting? SiteSetting { get; set; }

    public List<Product> Products { get; set; } = new();

    public List<Category> Categories { get; set; } = new();
}