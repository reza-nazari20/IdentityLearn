using Microsoft.AspNetCore.Identity;

namespace Identity.Models.Entities
{
    public class User : IdentityUser
    // کلاس مدل کاربر که از IdentityUser ارث‌بری می‌کند
    // IdentityUser کلاس پایه برای مدیریت کاربران در سیستم احراز هویت ASP.NET Core Identity است
    {
        public string FirstName { get; set; }
        // خاصیت FirstName برای نگهداری نام کاربر
        // این خاصیت علاوه بر خاصیت‌های موجود در کلاس پایه IdentityUser اضافه شده است

        public string LastName { get; set; }
        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        // این خاصیت علاوه بر خاصیت‌های موجود در کلاس پایه IdentityUser اضافه شده است
    }
}
