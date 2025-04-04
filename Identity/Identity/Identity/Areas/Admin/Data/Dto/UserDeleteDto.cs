﻿namespace Identity.Areas.Admin.Data.Dto
{
    // کلاس DTO برای نمایش اطلاعات کاربر در صفحه تأیید حذف
    public class UserDeleteDto
    {
        // خاصیت Id برای نگهداری شناسه منحصر به فرد کاربر
        public string Id { get; set; }

        // خاصیت FullName برای نمایش نام کامل کاربر در صفحه تأیید حذف
        public string FullName { get; set; }

        // خاصیت Email برای نمایش ایمیل کاربر در صفحه تأیید حذف
        public string Email { get; set; }

        // خاصیت UserName برای نمایش نام کاربری در صفحه تأیید حذف
        public string UserName { get; set; }
    }
}
