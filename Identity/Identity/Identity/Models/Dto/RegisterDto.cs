using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto
{
    // کلاس DTO برای دریافت اطلاعات ثبت‌نام کاربر جدید از فرم
    public class RegisterDto
    {
        // مشخص می‌کند که این فیلد ضروری است و نمی‌تواند خالی باشد
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        // مشخص می‌کند که این فیلد باید یک آدرس ایمیل معتبر باشد
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        // مشخص می‌کند که این فیلد از نوع رمز عبور است (برای نمایش مناسب در فرم)
        [DataType(DataType.Password)]
        // مشخص می‌کند که مقدار این فیلد باید با مقدار فیلد Password مطابقت داشته باشد
        [Compare(nameof(Password))]
        public string ConfrimPassword { get; set; }
    }
}
