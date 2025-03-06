using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto
{
    public class LoginDto
    // کلاس DTO برای دریافت اطلاعات ورود کاربر از فرم
    {
        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [EmailAddress]
        // مشخص می‌کند که این فیلد باید یک آدرس ایمیل معتبر باشد
        public string UserName { get; set; }
        // خاصیت UserName برای نگهداری نام کاربری (ایمیل) کاربر

        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [DataType(DataType.Password)]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        public string Password { get; set; }
        // خاصیت Password برای نگهداری رمز عبور کاربر

        [Display(Name = "Remember me")]
        // مشخص می‌کند که نام نمایشی این فیلد در فرم "Remember me" است
        public bool IsPersistent { get; set; } = false;
        // خاصیت IsPersistent برای تعیین اینکه آیا ورود کاربر به سیستم ذخیره شود یا خیر (به خاطر سپردن ورود)
        // مقدار پیش‌فرض false است

        public string ReturnUrl { get; set; }
        // خاصیت ReturnUrl برای نگهداری آدرس صفحه‌ای که کاربر باید پس از ورود به آن هدایت شود
    }
}
