using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Models.ViewModels;

namespace OnlineShop.Controllers;

public class HomeController(
    AppDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    // ریست رمز ادمین (فقط برای توسعه)
    public async Task<IActionResult> ResetAdmin()
    {
        var user = await userManager.FindByEmailAsync("ghaemi.nastaran1995@gmail.com");

        if (user == null)
            return Content("User not found");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var result = await userManager.ResetPasswordAsync(
            user,
            token,
            "Admin123");

        if (result.Succeeded)
            return Content("Password Changed");

        return Content(string.Join(",", result.Errors.Select(x => x.Description)));
    }

    // صفحه اصلی
    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            SiteSetting = await context.SiteSettings
                .FirstOrDefaultAsync(),

            Products = await context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Take(8)
                .ToListAsync(),

            Categories = await context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync()
        };

        return View(model);
    }

    // درباره ما
    public async Task<IActionResult> About()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        return View(setting);
    }

    // تماس با ما
    public async Task<IActionResult> Contact()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        return View(setting);
    }

    // قوانین
    public async Task<IActionResult> Privacy()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        return View(setting);
    }
}