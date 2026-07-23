using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var cart = GetCart();

            return View(cart);
        }



        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId);


            if (product == null)
                return NotFound();


            var cart = GetCart();


            var item = cart.FirstOrDefault(x => x.ProductId == productId);


            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = quantity
                });
            }


            SaveCart(cart);


            return RedirectToAction("Index");
        }




        public IActionResult Remove(int id)
        {
            var cart = GetCart();


            var item = cart.FirstOrDefault(x => x.ProductId == id);


            if (item != null)
            {
                cart.Remove(item);
            }


            SaveCart(cart);


            return RedirectToAction("Index");
        }





        [HttpGet]
        public IActionResult Update(int id, int quantity)
        {
            var cart = GetCart();


            var item = cart.FirstOrDefault(x => x.ProductId == id);


            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }


            SaveCart(cart);


            return RedirectToAction("Index");
        }





        private List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString("Cart");


            if (session == null)
                return new List<CartItem>();


            return JsonConvert.DeserializeObject<List<CartItem>>(session)
                   ?? new List<CartItem>();
        }





        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(
                "Cart",
                JsonConvert.SerializeObject(cart)
            );
        }

    }
}