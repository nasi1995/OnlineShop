using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Models.ViewModels;

namespace OnlineShop.Controllers;

[Authorize]
public class ProfileController(
    AppDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{

    // داشبورد پروفایل
    public async Task<IActionResult> Dashboard()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");


        var orders = await context.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();


        ViewBag.User = user;


        return View(orders);
    }




    // جزئیات سفارش
    public async Task<IActionResult> OrderDetails(int id)
    {
        var user = await userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Login", "Account");



        var order = await context.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == user.Id);



        if (order == null)
            return NotFound();



        return View(order);
    }






    // ============================
    // تغییر آدرس
    // ============================


    [HttpGet]
    public async Task<IActionResult> EditAddress()
    {
        var user = await userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Login", "Account");


        return View(user);
    }





    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAddress(ApplicationUser model)
    {
        var user = await userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Login", "Account");



        user.Province = model.Province;
        user.City = model.City;
        user.Address = model.Address;
        user.PostalCode = model.PostalCode;
        user.Mobile = model.Mobile;
        user.PhoneNumber = model.Mobile;



        var result = await userManager.UpdateAsync(user);



        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }


            return View(model);
        }



        await signInManager.RefreshSignInAsync(user);



        TempData["Success"] =
            "آدرس با موفقیت بروزرسانی شد.";



        return RedirectToAction(nameof(Dashboard));
    }







    // ============================
    // تغییر عکس پروفایل
    // ============================


    [HttpGet]
    public IActionResult ChangePhoto()
    {
        return View();
    }






    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePhoto(IFormFile Photo)
    {
        var user = await userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Login", "Account");



        if (Photo == null || Photo.Length == 0)
        {
            TempData["Error"] =
                "لطفاً یک تصویر انتخاب کنید.";


            return RedirectToAction(nameof(ChangePhoto));
        }




        var extension =
            Path.GetExtension(Photo.FileName).ToLower();



        var allowed =
            new[] { ".jpg", ".jpeg", ".png", ".webp" };



        if (!allowed.Contains(extension))
        {
            TempData["Error"] =
                "فرمت تصویر مجاز نیست.";


            return RedirectToAction(nameof(ChangePhoto));
        }




        var fileName =
            Guid.NewGuid()
            + extension;



        var folder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images/profile"
            );



        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);




        var path =
            Path.Combine(folder, fileName);




        using (var stream = new FileStream(path, FileMode.Create))
        {
            await Photo.CopyToAsync(stream);
        }




        user.ProfileImage = fileName;




        var result =
            await userManager.UpdateAsync(user);




        if (!result.Succeeded)
        {
            TempData["Error"] =
                "خطا در ذخیره تصویر.";


            return RedirectToAction(nameof(ChangePhoto));
        }




        await signInManager.RefreshSignInAsync(user);



        TempData["Success"] =
            "تصویر پروفایل تغییر کرد.";



        return RedirectToAction(nameof(Dashboard));
    }







    // ============================
    // تغییر شماره موبایل
    // ============================


    [HttpGet]
    public async Task<IActionResult> ChangePhoneNumber()
    {
        var user = await userManager.GetUserAsync(User);


        if (user == null)
            return RedirectToAction("Login", "Account");


        return View(user);
    }







    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePhoneNumber(string mobile)
    {
        var user = await userManager.GetUserAsync(User);



        if (user == null)
            return RedirectToAction("Login", "Account");



        user.Mobile = mobile;
        user.PhoneNumber = mobile;




        await userManager.UpdateAsync(user);



        await signInManager.RefreshSignInAsync(user);



        TempData["Success"] =
            "شماره موبایل با موفقیت تغییر کرد.";



        return RedirectToAction(nameof(Dashboard));
    }








    // ============================
    // تغییر رمز عبور
    // ============================


    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }







    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {

        if (!ModelState.IsValid)
            return View(model);



        var user =
            await userManager.GetUserAsync(User);




        if (user == null)
            return RedirectToAction("Login", "Account");




        var result =
            await userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );





        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }


            return View(model);
        }





        await userManager.UpdateSecurityStampAsync(user);



        await signInManager.RefreshSignInAsync(user);




        TempData["Success"] =
            "رمز عبور با موفقیت تغییر کرد.";




        return RedirectToAction(nameof(Dashboard));
    }

}