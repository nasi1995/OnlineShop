using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.ViewModels;

public class CreateProductViewModel
{
    [Display(Name = "نام محصول")]
    [Required]
    public string Name { get; set; }

    [Display(Name = "توضیحات")]
    public string? Description { get; set; }

    [Display(Name = "قیمت")]
    public decimal Price { get; set; }

    [Display(Name = "قیمت قبل")]
    public decimal? OldPrice { get; set; }

    [Display(Name = "دسته بندی")]
    public int CategoryId { get; set; }

    [Display(Name = "تصویر محصول")]
    public IFormFile? Image { get; set; }

    [Display(Name = "وزن")]
    public string? Weight { get; set; }

    [Display(Name = "نوع محصول")]
    public string? ProductType { get; set; }

    [Display(Name = "طعم")]
    public string? Flavor { get; set; }

    [Display(Name = "تعداد نظرات")]
    public int ReviewCount { get; set; }
}