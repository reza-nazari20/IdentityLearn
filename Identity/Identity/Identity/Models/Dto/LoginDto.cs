using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto
{
    // کلاس DTO برای دریافت اطلاعات ورود کاربر از فرم
    public class LoginDto
    {
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [Required]
        // مشخص می‌کند که این فیلد باید یک آدرس ایمیل معتبر باشد
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // مشخص می‌کند که نام نمایشی این فیلد در فرم "Remember me" است
        [Display(Name = "Remember me")]
        // خاصیت IsPersistent برای تعیین اینکه آیا ورود کاربر به سیستم ذخیره شود یا خیر (به خاطر سپردن ورود)
        public bool IsPersistent { get; set; } = false;

        // خاصیت ReturnUrl برای نگهداری آدرس صفحه‌ای که کاربر باید پس از ورود به آن هدایت شود
        public string ReturnUrl { get; set; }
    }
}
