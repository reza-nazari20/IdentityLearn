namespace Identity.Areas.Admin.Data.Dto
{
    // کلاس DTO برای نمایش اطلاعات کاربران در لیست کاربران
    public class UserListDto
    {
        // خاصیت Id برای نگهداری شناسه منحصر به فرد کاربر
        public string Id { get; set; }

        // خاصیت FirstName برای نگهداری نام کاربر
        public string FirstName { get; set; }

        // خاصیت LastName برای نگهداری نام خانوادگی کاربر
        public string LastName { get; set; }

        // خاصیت UserName برای نگهداری نام کاربری
        public string UserName { get; set; }

        // خاصیت PhoneNumber برای نگهداری شماره تلفن کاربر
        public string PhoneNumber { get; set; }

        // خاصیت EmailConfirmed برای نگهداری وضعیت تأیید ایمیل کاربر (آیا ایمیل تأیید شده است یا خیر)
        public bool EmailConfirmed { get; set; }

        // خاصیت AccessFailedCount برای نگهداری تعداد دفعات ورود ناموفق کاربر
        public int AccessFailedCount { get; set; }
    }
}
