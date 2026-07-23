using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly AppDbContext context;

    public FooterViewComponent(AppDbContext context)
    {
        this.context = context;
    }


    public async Task<IViewComponentResult> InvokeAsync()
    {
        var setting = await context.SiteSettings
            .FirstOrDefaultAsync();

        return View(setting);
    }
}