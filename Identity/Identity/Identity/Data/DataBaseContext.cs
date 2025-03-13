using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    // کلاس زمینه پایگاه داده که از IdentityDbContext ارث‌بری می‌کند با سه پارامتر ژنریک:
    // User: کلاس مدل کاربر سفارشی
    // Role: کلاس مدل نقش سفارشی
    // string: نوع داده کلید اصلی برای کاربر و نقش (در اینجا string)
    public class DataBaseContext : IdentityDbContext<User, Role, string>
    {
        // سازنده کلاس که تنظیمات اتصال به پایگاه داده را دریافت می‌کند و به کلاس پایه ارسال می‌کند
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        // متد بازنویسی شده برای پیکربندی مدل Entity Framework در زمان ساخت مدل
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تعریف کلید اصلی ترکیبی برای جدول IdentityUserLogin
            // کلید اصلی از فیلدهای ProviderKey و LoginProvider تشکیل شده است
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.ProviderKey, p.LoginProvider });

            // تعریف کلید اصلی ترکیبی برای جدول IdentityUserRole
            // کلید اصلی از فیلدهای UserId و RoleId تشکیل شده است
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });

            // تعریف کلید اصلی ترکیبی برای جدول IdentityUserToken
            // کلید اصلی از فیلدهای UserId و LoginProvider و Name تشکیل شده است
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });

            // تنظیم Entity Framework برای نادیده گرفتن (عدم ایجاد ستون) خاصیت NormalizedEmail در مدل User
            //modelBuilder.Entity<User>().Ignore(p => p.NormalizedEmail);
        }
    }
}
