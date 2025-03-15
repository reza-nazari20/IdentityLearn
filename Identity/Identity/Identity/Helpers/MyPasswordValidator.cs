using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    // کلاس اعتبارسنجی سفارشی برای رمزهای عبور که از رابط IPasswordValidator پیاده‌سازی می‌کند
    public class MyPasswordValidator : IPasswordValidator<User>
    {
        // لیستی از رمزهای عبور رایج و ضعیف که استفاده از آنها مجاز نیست
        List<string> CommonPassword = new List<string>()
   {
       "123456","zxcV@1234","abcd1234+","10203040","password","qwerty"
   };

        // متد اعتبارسنجی رمز عبور که به صورت آسنکرون اجرا می‌شود
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            // بررسی می‌کند آیا رمز عبور وارد شده جزو رمزهای عبور رایج و ضعیف است یا خیر
            if (CommonPassword.Contains(password))
            {
                // ایجاد یک نتیجه خطا با پیام مناسب در صورتی که رمز عبور ضعیف باشد
                var result = IdentityResult.Failed(new IdentityError
                {
                    // کد خطا برای شناسایی نوع خطا
                    Code = nameof(CommonPassword),
                    // پیام خطا به زبان فارسی برای نمایش به کاربر
                    Description = "رمز عبوری که وارد کردید ضعیف است و برای گروه های هکری شناسایی شده است،لطفا رمز عبور قوی تری وارد کنید"
                });
                // بازگرداندن نتیجه خطا به صورت یک Task
                return Task.FromResult(result);
            }
            // در صورتی که رمز عبور در لیست رمزهای رایج و ضعیف نباشد، اعتبارسنجی موفقیت‌آمیز است
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
