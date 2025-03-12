using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto.Account
{
    // کلاس DTO برای دریافت و اعتبارسنجی اطلاعات شماره تلفن از فرم
    public class SetPhoneNumberDto
    {
        // مشخص می‌کند که فیلد PhoneNumber اجباری است و نمی‌تواند خالی باشد
        [Required]

        // اعتبار سنجی تلفن براساس تلفن ایران ؛یعنی باید با +98 یا 0 شروع شود و پس از آن عدد 9 باشد و پس از آن 9 رقم وارد شود
        [RegularExpression(@"(\+98|0)?9\d{9}")]

        // خاصیت برای نگهداری شماره تلفن وارد شده توسط کاربر
        public string PhoneNumber { get; set; }
    }
}
