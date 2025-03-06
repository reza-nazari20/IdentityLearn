namespace Identity.Areas.Admin.Data.Dto
{
    public class UserEditDto
    // کلاس DTO برای دریافت و انتقال داده‌های ویرایش کاربر
    {
        public string Id { get; set; }
        // خاصیت Id برای نگهداری شناسه منحصر به فرد کاربر
        public string FirstName { get; set; }
        // خاصیت FirstName برای نگهداری نام کاربر
        public string LastName { get; set; }
        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        public string Email { get; set; }
        // خاصیت Email برای نگهداری آدرس ایمیل کاربر
        public string UserName { get; set; }
        // خاصیت UserName برای نگهداری نام کاربری
        public string PhoneNumber { get; set; }
        // خاصیت PhoneNumber برای نگهداری شماره تلفن کاربر
    }
}
