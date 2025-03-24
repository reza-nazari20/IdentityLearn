using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    // کلاس سفارشی برای تعریف یک قانون دسترسی بر اساس مقدار یک کلیم
    public class UserCreditRequerment : IAuthorizationRequirement
    {
        // مقدار مورد نیاز برای اعتبارسنجی کاربر
        public int Credit { get; set; }

        // سازنده کلاس برای تعیین مقدار مورد نیاز برای اعتبارسنجی کاربر 
        public UserCreditRequerment(int credit)
        {
            // تعیین مقدار مورد نیاز برای اعتبارسنجی کاربر
            Credit = credit;
        }
    }

    // کلاس سفارشی برای اعتبارسنجی کاربر بر اساس مقدار کلیم "Cradit" در هویت کاربر
    public class UserCreditHandler : AuthorizationHandler<UserCreditRequerment>
    {
        // متد اجباری برای اعتبارسنجی کاربر بر اساس مقدار کلیم "Cradit" در هویت کاربر 
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserCreditRequerment requirement)
        {
            // یافتن کلیم "Cradit" در هویت کاربر
            var claim = context.User.FindFirst("Cradit");

            // بررسی اینکه آیا کلیم "Cradit" در هویت کاربر وجود دارد یا خیر
            if (claim != null)
            {
                // تبدیل مقدار کلیم "Cradit" به عدد صحیح
                int cradit = int.Parse(claim?.Value);

                // بررسی اینکه آیا مقدار کلیم "Cradit" بیشتر یا مساوی مقدار مورد نیاز برای اعتبارسنجی کاربر است یا خیر
                if (cradit >= requirement.Credit)
                {
                    // اعلام موفقیت اعتبارسنجی کاربر
                    context.Succeed(requirement);
                }
            }

            // اعلام اعتبارسنجی کاربر
            return Task.CompletedTask;
        }
    }
}
