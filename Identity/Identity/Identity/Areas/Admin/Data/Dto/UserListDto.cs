namespace Identity.Areas.Admin.Data.Dto
{
    public class UserListDto
    // کلاس DTO برای نمایش اطلاعات کاربران در لیست کاربران
    {
        public string Id { get; set; }
        // خاصیت Id برای نگهداری شناسه منحصر به فرد کاربر
        public string FirstName { get; set; }
        // خاصیت FirstName برای نگهداری نام کاربر
        public string LastName { get; set; }
        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        public string UserName { get; set; }
        // خاصیت UserName برای نگهداری نام کاربری
        public string PhoneNumber { get; set; }
        // خاصیت PhoneNumber برای نگهداری شماره تلفن کاربر
        public bool EmailConfirmed { get; set; }
        // خاصیت EmailConfirmed برای نگهداری وضعیت تأیید ایمیل کاربر (آیا ایمیل تأیید شده است یا خیر)
        public int AccessFailedCount { get; set; }
        // خاصیت AccessFailedCount برای نگهداری تعداد دفعات ورود ناموفق کاربر
    }
}
