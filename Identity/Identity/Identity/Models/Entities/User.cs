using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Models.Entities
{
    // کلاس مدل کاربر که از IdentityUser ارث‌بری می‌کند
    // IdentityUser کلاس پایه برای مدیریت کاربران در سیستم احراز هویت ASP.NET Core Identity است
    public class User : IdentityUser
    {
        // خاصیت FirstName برای نگهداری نام کاربر
        // این خاصیت علاوه بر خاصیت‌های موجود در کلاس پایه IdentityUser اضافه شده است
        public string FirstName { get; set; }


        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        // این خاصیت علاوه بر خاصیت‌های موجود در کلاس پایه IdentityUser اضافه شده است
        public string LastName { get; set; }

        // مجموعه‌ای از بلاگ‌های متعلق به این کاربر
        // نشان‌دهنده رابطه یک به چند بین کاربر و بلاگ‌ها
        // این رابطه امکان دسترسی به تمام بلاگ‌های یک کاربر را فراهم می‌کند
        public ICollection<Blog> Blogs { get; set; }
    }
}
