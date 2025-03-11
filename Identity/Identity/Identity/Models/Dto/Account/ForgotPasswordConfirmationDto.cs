using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto.Account
{
    // کلاس DTO برای دریافت اطلاعات فراموشی رمز عبور
    public class ForgotPasswordConfirmationDto
    {
        // مشخص کردن اینکه مقدار این فیلد الزامی است
        [Required]
        // بررسی صحت فرمت ایمیل وارد شده
        [EmailAddress]

        // دریافت ایمیل کاربر جهت ارسال لینک بازنشانی رمز عبور
        public string Email { get; set; }
    }

}
