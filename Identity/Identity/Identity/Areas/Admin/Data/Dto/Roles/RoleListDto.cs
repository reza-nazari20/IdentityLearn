namespace Identity.Areas.Admin.Data.Dto.Roles
{
    // کلاس DTO برای نمایش اطلاعات نقش‌ها در لیست نقش‌ها
    public class RoleListDto
    {
        // خاصیت Id برای نگهداری شناسه منحصر به فرد نقش
        public string Id { get; set; }

        // خاصیت Name برای نگهداری نام نقش
        public string Name { get; set; }

        // خاصیت Description برای نگهداری توضیحات نقش
        public string Description { get; set; }
    }
}
