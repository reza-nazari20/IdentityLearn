using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto.Account
{
    // کلاس DTO برای دریافت اطلاعات تنظیم مجدد رمز عبور
    public class ResetPasswordDto
    {
        // مشخص کردن اینکه مقدار این فیلد الزامی است
        [Required]
        // تعیین نوع داده برای رمز عبور
        [DataType(DataType.Password)]
        // مقدار رمز عبور جدید که کاربر وارد می‌کند
        public string Password { get; set; }

        [Required] 
        [DataType(DataType.Password)]
        // بررسی اینکه مقدار این فیلد باید با فیلد Password یکسان باشد
        [Compare(nameof(Password))]
        // مقدار تأیید رمز عبور که باید با رمز عبور اصلی مطابقت داشته باشد
        public string ConfirmPassword { get; set; }

        // شناسه کاربر برای شناسایی وی هنگام بازنشانی رمز عبور
        public string UserId { get; set; }

        // توکن ارسال‌شده برای اعتبارسنجی درخواست بازنشانی رمز عبور
        public string Token { get; set; }
    }

}
