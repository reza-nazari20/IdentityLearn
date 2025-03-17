using Identity.Models.Entities;
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
}
