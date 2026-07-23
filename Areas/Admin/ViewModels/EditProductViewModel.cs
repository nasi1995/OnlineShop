using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.ViewModels;

public class EditProductViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "نام محصول")]
    public string Name { get; set; }

    [Display(Name = "توضیحات")]
    public string? Description { get; set; }

    [Display(Name = "قیمت")]
    public decimal Price { get; set; }

    [Display(Name = "قیمت قبل")]
    public decimal? OldPrice { get; set; }

    [Display(Name = "وزن")]
    public string? Weight { get; set; }

    [Display(Name = "نوع محصول")]
    public string? ProductType { get; set; }

    [Display(Name = "طعم")]
    public string? Flavor { get; set; }

    public int CategoryId { get; set; }

    public int ReviewCount { get; set; }

    public string? CurrentImage { get; set; }

    public IFormFile? Image { get; set; }
}