using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin;
using OnlineShop.Areas.Admin.ViewModels;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Models.ViewModels;

namespace OnlineShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController(
    AppDbContext context,
    UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            ProductsCount = await context.Products.CountAsync(),

            CategoriesCount = await context.Categories.CountAsync(),

            UsersCount = await userManager.Users.CountAsync(),

            OrdersCount = await context.Orders.CountAsync(),

            TotalSales = await context.Orders
         .Where(x => x.PaymentStatus == "Paid")
         .SumAsync(x => (decimal?)x.TotalPrice) ?? 0,

            LatestOrders = await context.Orders
         .OrderByDescending(x => x.CreatedAt)
         .Take(5)
         .ToListAsync(),

            LatestUsers = await userManager.Users
         .OrderByDescending(x => x.Id)
         .Take(5)
         .ToListAsync()
        };

        return View(model);
    }

public async Task<IActionResult> Products(string? search, int page = 1)
    {
    IQueryable<Product> query = context.Products
        .Include(p => p.Category);

    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();

        query = query.Where(p =>
            p.Name.Contains(search) ||
            (p.Description != null && p.Description.Contains(search)) ||
            (p.ProductType != null && p.ProductType.Contains(search)) ||
            (p.Flavor != null && p.Flavor.Contains(search)) ||
            p.Category.Name.Contains(search));
    }

    var products = await query
        .OrderByDescending(p => p.Id)
        .ToListAsync();

    ViewBag.Search = search;

    return View(products);
}
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.Categories = await context.Categories.ToListAsync();

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(model);
        }


        string? imageName = null;


        if (model.Image != null)
        {
            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/products"
            );


            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }


            imageName = Guid.NewGuid().ToString()
                + Path.GetExtension(model.Image.FileName);


            var filePath = Path.Combine(folder, imageName);


            using var stream = new FileStream(filePath, FileMode.Create);

            await model.Image.CopyToAsync(stream);
        }


        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            OldPrice = model.OldPrice,
            CategoryId = model.CategoryId,
            ImageUrl = "/images/products/" + imageName,
            Weight = model.Weight,
            ProductType = model.ProductType,
            Flavor = model.Flavor,
            ReviewCount = model.ReviewCount
        };


        context.Products.Add(product);

        await context.SaveChangesAsync();


        TempData["Success"] = "محصول با موفقیت اضافه شد.";

        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = await context.Categories
            .OrderBy(x => x.Name)
            .ToListAsync();

        var model = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            OldPrice = product.OldPrice,
            Weight = product.Weight,
            ProductType = product.ProductType,
            Flavor = product.Flavor,
            CategoryId = product.CategoryId,
            ReviewCount = product.ReviewCount,
            CurrentImage = product.ImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, EditProductViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(model);
        }

        var product = await context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        if (model.Image != null)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImage = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    product.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (System.IO.File.Exists(oldImage))
                    System.IO.File.Delete(oldImage);
            }

            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/products");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var imageName = Guid.NewGuid() +
                            Path.GetExtension(model.Image.FileName);

            var path = Path.Combine(folder, imageName);

            using var stream = new FileStream(path, FileMode.Create);

            await model.Image.CopyToAsync(stream);

            product.ImageUrl = "/images/products/" + imageName;
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.OldPrice = model.OldPrice;
        product.Weight = model.Weight;
        product.ProductType = model.ProductType;
        product.Flavor = model.Flavor;
        product.CategoryId = model.CategoryId;
        product.ReviewCount = model.ReviewCount;

        await context.SaveChangesAsync();

        TempData["Success"] = "محصول با موفقیت ویرایش شد.";

        return RedirectToAction(nameof(Products));
    }
    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return NotFound();

        return View(product);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        TempData["Success"] = "محصول حذف شد.";

        return RedirectToAction(nameof(Products));
    }
    public async Task<IActionResult> Orders(string? search)
    {
        IQueryable<Order> query = context.Orders
            .Include(x => x.User);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.FullName.Contains(search) ||
                x.Mobile.Contains(search));
        }

        var orders = await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        ViewBag.Search = search;

        return View(orders);
    }
    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await context.Orders
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
            return NotFound();

        return View(order);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePaymentStatus(int id, string status)
    {
        var order = await context.Orders.FindAsync(id);

        if (order == null)
            return NotFound();

        order.PaymentStatus = status;

        await context.SaveChangesAsync();

        TempData["Success"] = "وضعیت پرداخت بروزرسانی شد.";

        return RedirectToAction(nameof(OrderDetails), new { id });
    }

    public async Task<IActionResult> Users(string? search)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.FullName.Contains(search) ||
                x.Email!.Contains(search));
        }

        var users = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        ViewBag.Search = search;

        return View(users);
    }

    public async Task<IActionResult> UserDetails(string id)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return NotFound();

        ViewBag.OrdersCount = await context.Orders
            .CountAsync(x => x.UserId == user.Id);

        ViewBag.TotalOrders = await context.Orders
            .Where(x => x.UserId == user.Id)
            .SumAsync(x => (decimal?)x.TotalPrice) ?? 0;

        var roles = await userManager.GetRolesAsync(user);

        ViewBag.Role = roles.FirstOrDefault() ?? "User";

        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(string id, string role)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        var roles = await userManager.GetRolesAsync(user);

        if (roles.Any())
            await userManager.RemoveFromRolesAsync(user, roles);

        await userManager.AddToRoleAsync(user, role);

        TempData["Success"] = "نقش کاربر با موفقیت تغییر کرد.";

        return RedirectToAction(nameof(UserDetails), new { id });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser != null && currentUser.Id == id)
        {
            TempData["Error"] = "امکان حذف حساب کاربری خودتان وجود ندارد.";
            return RedirectToAction(nameof(Users));
        }

        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        await userManager.DeleteAsync(user);

        TempData["Success"] = "کاربر با موفقیت حذف شد.";

        return RedirectToAction(nameof(Users));
    }

    public IActionResult Settings()
    {
        return View();
    }

    public async Task<IActionResult> StoreSettings()
    {
        var model = await context.SiteSettings.FirstOrDefaultAsync();

        if (model == null)
        {
            model = new SiteSetting();

            context.SiteSettings.Add(model);

            await context.SaveChangesAsync();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StoreSettings(SiteSetting model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setting = await context.SiteSettings.FirstAsync();

        setting.SiteName = model.SiteName;
        setting.Description = model.Description;
        setting.Logo = model.Logo;
        setting.Favicon = model.Favicon;

        await context.SaveChangesAsync();

        TempData["Success"] = "اطلاعات فروشگاه با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(StoreSettings));
    }


    public async Task<IActionResult> ContactSettings()
    {
        var model = await context.SiteSettings.FirstOrDefaultAsync();

        if (model == null)
        {
            model = new SiteSetting();

            context.SiteSettings.Add(model);

            await context.SaveChangesAsync();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactSettings(SiteSetting model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        var setting = await context.SiteSettings
            .FirstOrDefaultAsync();


        if (setting == null)
        {
            context.SiteSettings.Add(model);
        }
        else
        {
            setting.Phone = model.Phone;
            setting.Mobile = model.Mobile;
            setting.Email = model.Email;
            setting.Address = model.Address;
        }


        await context.SaveChangesAsync();


        TempData["Success"] = "اطلاعات تماس با موفقیت ذخیره شد.";


        return RedirectToAction(nameof(ContactSettings));
    }

    public async Task<IActionResult> ShopInfo()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
        }

        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ShopInfo(SiteSetting model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            context.SiteSettings.Add(model);
        }
        else
        {
            setting.SiteName = model.SiteName;
            setting.Logo = model.Logo;
            setting.Favicon = model.Favicon;
            setting.Description = model.Description;
        }

        await context.SaveChangesAsync();

        TempData["Success"] = "اطلاعات فروشگاه با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(ShopInfo));
    }

    public async Task<IActionResult> SocialSettings()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
            await context.SaveChangesAsync();
        }

        return View(setting);
    }

    // ذخیره اطلاعات
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SocialSettings(SiteSetting model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
        }

        setting.Instagram = model.Instagram;
        setting.Telegram = model.Telegram;
        setting.WhatsApp = model.WhatsApp;

        await context.SaveChangesAsync();

        TempData["Success"] = "شبکه‌های اجتماعی با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(SocialSettings));
    }

    public async Task<IActionResult> Seo()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
            await context.SaveChangesAsync();
        }

        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Seo(SiteSetting model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
        }

        setting.MetaTitle = model.MetaTitle;
        setting.MetaDescription = model.MetaDescription;
        setting.Keywords = model.Keywords;

        await context.SaveChangesAsync();

        TempData["Success"] = "تنظیمات سئو با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(Seo));
    }

    public async Task<IActionResult> AboutUs()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
            await context.SaveChangesAsync();
        }

        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutUs(SiteSetting model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
        }

        setting.AboutUs = model.AboutUs;

        await context.SaveChangesAsync();

        TempData["Success"] = "متن درباره ما با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(AboutUs));
    }
    public async Task<IActionResult> ShipingSettings()
    {
        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
            await context.SaveChangesAsync();
        }

        return View(setting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ShipingSettings(SiteSetting model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setting = await context.SiteSettings.FirstOrDefaultAsync();

        if (setting == null)
        {
            setting = new SiteSetting();
            context.SiteSettings.Add(setting);
        }

        setting.ShippingCost = model.ShippingCost;
        setting.FreeShippingAmount = model.FreeShippingAmount;
        setting.WorkingHours = model.WorkingHours;

        await context.SaveChangesAsync();

        TempData["Success"] = "تنظیمات ارسال با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(ShipingSettings));
    }
}