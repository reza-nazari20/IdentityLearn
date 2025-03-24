using Identity.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    // کلاس سفارشی برای افزودن کلیم‌های اضافی به هویت کاربر
    public class AddMyClaims : UserClaimsPrincipalFactory<User>
    {
        // سازنده کلاس برای تزریق وابستگی‌های مورد نیاز
        public AddMyClaims(UserManager<User> userManager
            , IOptions<IdentityOptions> options) : base(userManager, options)
        {
        }

        // متد بازنویسی شده برای تولید کلیم‌های هویت کاربر
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            // فراخوانی متد پایه برای ایجاد کلیم‌های استاندارد هویت
            var identity = await base.GenerateClaimsAsync(user);

            // افزودن کلیم جدید "FullName" که ترکیب نام و نام خانوادگی کاربر است
            identity.AddClaim(new Claim("FullName", $"{user.FirstName}{user.LastName}"));

            // بازگرداندن هویت غنی شده با کلیم‌های اضافی
            return identity;
        }
    }

    // کلاس سفارشی برای تغییر و تحول کلیم‌های کاربر با پیاده‌سازی اینترفیس IClaimsTransformation
    public class AddClaim : IClaimsTransformation
    {
        // متد اجباری از اینترفیس IClaimsTransformation برای تغییر کلیم‌های کاربر در زمان اجرا
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // بررسی اینکه آیا شیء principal خالی نیست
            if (principal != null)
            {
                // تبدیل هویت اصلی کاربر به ClaimsIdentity برای دسترسی به کلیم‌ها
                var identity = principal.Identity as ClaimsIdentity;
                // بررسی اینکه آیا تبدیل با موفقیت انجام شده است
                if (identity != null)
                {
                    // افزودن یک کلیم جدید با نام "TestClaim" و مقدار "YES" به هویت کاربر
                    identity.AddClaim(new Claim("TestClaim", "YES", ClaimValueTypes.String));

                    // افزودن یک کلیم جدید با نام "Cradit" و مقدار "10000" به هویت کاربر
                    identity.AddClaim(new Claim("Cradit","10000",ClaimValueTypes.String)); 
                }
            }
            // بازگرداندن شیء principal تغییریافته به صورت یک Task
            return Task.FromResult(principal);
        }
    }
}
