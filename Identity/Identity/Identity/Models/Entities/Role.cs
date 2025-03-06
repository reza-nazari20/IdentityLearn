using Microsoft.AspNetCore.Identity;

namespace Identity.Models.Entities
{
    public class Role : IdentityRole
    // کلاس مدل نقش که از IdentityRole ارث‌بری می‌کند
    // IdentityRole کلاس پایه برای مدیریت نقش‌ها در سیستم احراز هویت ASP.NET Core Identity است
    {
        public string Description { get; set; }
        // خاصیت Description برای نگهداری توضیحات نقش
        // این خاصیت علاوه بر خاصیت‌های موجود در کلاس پایه IdentityRole اضافه شده است
    }
}
