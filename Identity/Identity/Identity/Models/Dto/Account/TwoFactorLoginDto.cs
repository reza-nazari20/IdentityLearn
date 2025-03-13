using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto.Account
{
    // مدل انتقال داده‌ها برای فرآیند احراز هویت دو مرحله‌ای
    public class TwoFactorLoginDto
    {
        // کد تأیید وارد شده توسط کاربر - این فیلد اجباری است
        [Required]
        public string Code { get; set; }

        // مشخص می‌کند آیا اطلاعات ورود کاربر ذخیره شود یا خیر (به خاطر سپردن ورود)
        public bool IsPersistent { get; set; }

        // نوع روش احراز هویت دو مرحله‌ای (مانند "Email" یا "Phone")
        public string providers { get; set; }
    }
}
