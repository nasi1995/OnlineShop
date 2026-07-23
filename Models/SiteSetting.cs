namespace OnlineShop.Models;

public class SiteSetting
{
    public int Id { get; set; }

    // اطلاعات فروشگاه
    public string SiteName { get; set; } = "";
    public string? Logo { get; set; }
    public string? Favicon { get; set; }
    public string? Description { get; set; }

    // تماس
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    // شبکه‌های اجتماعی
    public string? Instagram { get; set; }
    public string? Telegram { get; set; }
    public string? WhatsApp { get; set; }

    // سئو
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }

    // درباره ما
    public string? AboutUs { get; set; }

    // صفحه اصلی
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
    public string? HeroImage { get; set; }

    // ارسال
    public decimal ShippingCost { get; set; }
    public decimal FreeShippingAmount { get; set; }
    public string? WorkingHours { get; set; }

    // پرداخت
    public string? MerchantId { get; set; }
    public bool Sandbox { get; set; }
}