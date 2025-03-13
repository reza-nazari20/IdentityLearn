using System.IO;

namespace Identity.Models.Dto.Account
{
    // مدل انتقال داده‌ها برای نمایش اطلاعات حساب کاربری
    public class MyAccountInfoDto
    {
        // شناسه یکتای کاربر در سیستم
        public string Id { get; set; }

        // نام کامل کاربر (ترکیب نام و نام خانوادگی)
        public string FullName { get; set; }

        // نام کاربری فرد در سیستم
        public string UserName { get; set; }

        // آدرس ایمیل کاربر
        public string Email { get; set; }

        // شماره تلفن کاربر
        public string PhoneNumber { get; set; }

        // نشان می‌دهد آیا ایمیل کاربر تأیید شده است یا خیر
        public bool EmailConfirmed { get; set; }

        // نشان می‌دهد آیا شماره تلفن کاربر تأیید شده است یا خیر
        public bool PhoneNumberConfirmed { get; set; }

        // نشان می‌دهد آیا قابلیت احراز هویت دو مرحله‌ای برای کاربر فعال است یا خیر
        public bool TowFactorEnable { get; set; }
    }
}
