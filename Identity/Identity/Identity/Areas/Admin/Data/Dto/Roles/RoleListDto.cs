namespace Identity.Areas.Admin.Data.Dto.Roles
{
    public class RoleListDto
    // کلاس DTO برای نمایش اطلاعات نقش‌ها در لیست نقش‌ها
    {
        public string Id { get; set; }
        // خاصیت Id برای نگهداری شناسه منحصر به فرد نقش
        public string Name { get; set; }
        // خاصیت Name برای نگهداری نام نقش
        public string Description { get; set; }
        // خاصیت Description برای نگهداری توضیحات نقش
    }
}
