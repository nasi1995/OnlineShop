using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.Controllers;

[Authorize(Roles = "Admin,Writer")]
public class ProductsController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
                                    .Include(x => x.Category)
                                    .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await context.Products
                                   .Include(x => x.Category)
                                   .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return NotFound();

        return View(product);
    }
}