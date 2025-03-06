using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto
{
    public class RegisterDto
    // کلاس DTO برای دریافت اطلاعات ثبت‌نام کاربر جدید از فرم
    {
        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        public string FirstName { get; set; }
        // خاصیت FirstName برای نگهداری نام کاربر

        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        public string LastName { get; set; }
        // خاصیت LastName برای نگهداری نام خانوادگی کاربر

        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [EmailAddress]
        // مشخص می‌کند که این فیلد باید یک آدرس ایمیل معتبر باشد
        public string Email { get; set; }
        // خاصیت Email برای نگهداری آدرس ایمیل کاربر

        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [DataType(DataType.Password)]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        public string Password { get; set; }
        // خاصیت Password برای نگهداری رمز عبور کاربر

        [Required]
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [DataType(DataType.Password)]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        [Compare(nameof(Password))]
        // مشخص می‌کند که مقدار این فیلد باید با مقدار فیلد Password مطابقت داشته باشد
        public string ConfrimPassword { get; set; }
        // خاصیت ConfrimPassword برای تأیید رمز عبور وارد شده (باید با Password مطابقت داشته باشد)
    }
}
