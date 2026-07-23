using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers;

[Authorize]
public class PaymentController(
    AppDbContext context,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager) : Controller
{
    private readonly HttpClient _client = httpClientFactory.CreateClient();

    [HttpGet]
    public IActionResult Index()
    {
        var cart = GetCart();

        if (!cart.Any())
            return RedirectToAction("Index", "Cart");

        return View(new Order());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Order order)
    {
        if (!ModelState.IsValid)
            return View(order);

        var cart = GetCart();

        if (!cart.Any())
            return RedirectToAction("Index", "Cart");


        var user = await userManager.GetUserAsync(User);

        order.UserId = user?.Id;

        order.TotalPrice = cart.Sum(x => x.Price * x.Quantity);

        order.CreatedAt = DateTime.Now;

        order.PaymentStatus = "Pending";


        order.Items = cart.Select(x => new OrderItem
        {
            ProductId = x.ProductId,
            ProductName = x.Name,
            Price = x.Price,
            Quantity = x.Quantity

        }).ToList();


        context.Orders.Add(order);

        await context.SaveChangesAsync();


        HttpContext.Session.SetInt32(
            "OrderId",
            order.Id
        );


        var request = new
        {
            merchant_id = configuration["Zarinpal:MerchantId"],

            amount = (int)order.TotalPrice * 10,

            description = "خرید محصولات نبات",

            callback_url = configuration["Zarinpal:CallbackUrl"]
        };


        var response = await _client.PostAsJsonAsync(
            "https://sandbox.zarinpal.com/pg/v4/payment/request.json",
            request
        );

        var json = await response.Content.ReadAsStringAsync();

        var result = System.Text.Json.JsonSerializer.Deserialize<ZarinpalResponse>(json);

        if (result?.Data?.Code == 100 &&
            !string.IsNullOrWhiteSpace(result.Data.Authority))
        {
            return Redirect(
                $"https://sandbox.zarinpal.com/pg/StartPay/{result.Data.Authority}"
            );
        }

        return Content(json);
    }



    public async Task<IActionResult> Verify(
        string authority,
        string status)
    {
        var orderId = HttpContext.Session.GetInt32("OrderId");


        if (orderId == null)
            return View("Failed");


        var order = await context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == orderId.Value);


        if (order == null)
            return View("Failed");


        if (status != "OK")
        {
            order.PaymentStatus = "Failed";

            await context.SaveChangesAsync();

            return View("Failed");
        }

        var verifyRequest = new
        {
            merchant_id = configuration["Zarinpal:MerchantId"],
            amount = (int)order.TotalPrice * 10,
            authority = authority
        };

        var response = await _client.PostAsJsonAsync(
            "https://sandbox.zarinpal.com/pg/v4/payment/verify.json",
            verifyRequest);

        var json = await response.Content.ReadAsStringAsync();

        var result = System.Text.Json.JsonSerializer.Deserialize<ZarinpalVerifyResponse>(json);

        // این قسمت بعداً Verify واقعی زرین‌پال را انجام می‌دهیم.
        if (result?.Data?.Code == 100 || result?.Data?.Code == 101)
        {
            order.PaymentStatus = "Paid";

            await context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("OrderId");

            ViewBag.RefId = result.Data.RefId;

            return View("Success", order);
        }

        order.PaymentStatus = "Failed";

        await context.SaveChangesAsync();

        return View("Failed");

        await context.SaveChangesAsync();


        HttpContext.Session.Remove("Cart");
        HttpContext.Session.Remove("OrderId");


        return View("Success", order);
    }



    private List<CartItem> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");


        if (session == null)
            return new List<CartItem>();


        return JsonConvert.DeserializeObject<List<CartItem>>(session)
               ?? new List<CartItem>();
    }
}