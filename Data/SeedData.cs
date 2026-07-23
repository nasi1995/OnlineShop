using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data;

public static class SeedData
{
    public static async Task Initialize(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // ساخت Role ها
        string[] roles =
        {
            "Admin",
            "Writer"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }


        // =========================
        // ساخت کاربر ادمین
        // =========================

        var adminEmail = "ghaemi.nastaran1995@gmail.com";

        var admin = await userManager.FindByEmailAsync(adminEmail);


        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "مدیر سایت",
                EmailConfirmed = true
            };


            await userManager.CreateAsync(
                admin,
                "Admin123");
        }
        else
        {
            // ریست پسورد ادمین موجود
            var token = await userManager.GeneratePasswordResetTokenAsync(admin);

            await userManager.ResetPasswordAsync(
                admin,
                token,
                "Admin123");
        }


        // اضافه کردن نقش Admin
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            await userManager.AddToRoleAsync(
                admin,
                "Admin");
        }



        // =========================
        // ساخت نویسنده
        // =========================

        var writerEmail = "writer@nabat.com";

        var writer = await userManager.FindByEmailAsync(writerEmail);


        if (writer == null)
        {
            writer = new ApplicationUser
            {
                UserName = writerEmail,
                Email = writerEmail,
                FullName = "نویسنده",
                EmailConfirmed = true
            };


            await userManager.CreateAsync(
                writer,
                "Writer12345");
        }


        // اضافه کردن نقش Writer
        if (!await userManager.IsInRoleAsync(writer, "Writer"))
        {
            await userManager.AddToRoleAsync(
                writer,
                "Writer");
        }
    }
}