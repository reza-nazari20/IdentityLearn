using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Identity.Areas.Admin.Data.Dto.Roles
{
    // کلاس DTO برای نگهداری و انتقال داده‌های مربوط به افزودن نقش به کاربر
    public class AddUserRoleDto
    {
        // خاصیت Id برای نگهداری شناسه کاربری که نقش به آن اضافه می‌شود
        public string Id { get; set; }

        // خاصیت Role برای نگهداری نام نقشی که قرار است به کاربر اضافه شود
        public string Role { get; set; }

        // خاصیت FullName برای نمایش نام کامل کاربر در فرم
        public string FullName { get; set; }

        // خاصیت Email برای نمایش ایمیل کاربر در فرم
        public string Email { get; set; }

        // خاصیت Roles که لیستی از SelectListItem‌ها را برای نمایش در دراپ‌داون نقش‌ها نگهداری می‌کند
        public List<SelectListItem> Roles { get; set; }
    }
}
