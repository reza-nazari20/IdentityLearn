using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Dto.Account
{
    // کلاس DTO برای دریافت و اعتبارسنجی اطلاعات تایید شماره تلفن
    public class VerifyPhoneNumberDto
    {
        // خاصیت برای نگهداری شماره تلفنی که باید تایید شود
        public string PhoneNumber { get; set; }

        // مشخص می‌کند که فیلد Code اجباری است و نمی‌تواند خالی باشد
        [Required]

        // تعیین حداکثر طول مجاز برای کد تایید (6 کاراکتر)
        [MaxLength(6)]

        // تعیین حداقل طول مجاز برای کد تایید (6 کاراکتر)
        [MinLength(6)]

        // خاصیت برای نگهداری کد تایید وارد شده توسط کاربر
        public string Code { get; set; }
    }
}
