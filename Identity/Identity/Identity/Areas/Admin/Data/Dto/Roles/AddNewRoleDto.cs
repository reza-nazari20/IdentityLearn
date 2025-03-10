namespace Identity.Areas.Admin.Data.Dto.Roles
{
    // کلاس DTO برای دریافت اطلاعات نقش جدید از فرم
    public class AddNewRoleDto
    {
        // خاصیت Name برای نگهداری نام نقش جدید
        public string Name { get; set; }

        // خاصیت Description برای نگهداری توضیحات نقش جدید
        public string Description { get; set; }
    }
}
