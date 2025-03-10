namespace Identity.Areas.Admin.Data.Dto
{
    // کلاس DTO برای دریافت و انتقال داده‌های ویرایش کاربر
    public class UserEditDto
    {
        // خاصیت Id برای نگهداری شناسه منحصر به فرد کاربر
        public string Id { get; set; }

        // خاصیت FirstName برای نگهداری نام کاربر
        public string FirstName { get; set; }

        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        public string LastName { get; set; }

        // خاصیت Email برای نگهداری آدرس ایمیل کاربر
        public string Email { get; set; }

        // خاصیت UserName برای نگهداری نام کاربری
        public string UserName { get; set; }

        // خاصیت PhoneNumber برای نگهداری شماره تلفن کاربر
        public string PhoneNumber { get; set; }
    }
}
