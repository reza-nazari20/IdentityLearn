using Identity.Models.Dto.Blog;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    // کلاس نیازمندی (Requirement) برای احراز مجوز بلاگ
    // این کلاس یک اینترفیس خالی است که نشان‌دهنده یک نیازمندی احراز مجوز است
    public class BlogRequirement : IAuthorizationRequirement
    {
    }

    // کلاس هندلر (Handler) برای بررسی مجوز ویرایش بلاگ
    // این کلاس مسئول تعیین اینکه آیا کاربر جاری مجاز به ویرایش بلاگ هست یا خیر
    public class IsBlogForUserAuthorizationHandler : AuthorizationHandler<BlogRequirement, BlogDto>
    {
        // متد اصلی برای بررسی مجوز
        // این متد به صورت غیرهمزمان (Async) اجرا می‌شود
        protected override Task HandleRequirementAsync(
            // کانتکست احراز مجوز
            AuthorizationHandlerContext context,
            // نیازمندی مورد بررسی
            BlogRequirement requirement,
            // منبع (در اینجا یک BlogDto) که مجوز آن بررسی می‌شود
            BlogDto resource)
        {
            // بررسی اینکه آیا نام کاربری کاربر جاری با نام کاربری نویسنده بلاگ مطابقت دارد
            if (context.User.Identity?.Name == resource.UserName)
            {
                // اگر مطابقت داشت، مجوز تایید می‌شود
                context.Succeed(requirement);
            }

            // پایان عملیات و بازگرداندن یک Task کامل شده
            return Task.CompletedTask;
        }
    }
}
