namespace Identity.Areas.Admin.Data.Dto.Roles
{
    public class AddNewRoleDto
    // کلاس DTO برای دریافت اطلاعات نقش جدید از فرم
    {
        public string Name { get; set; }
        // خاصیت Name برای نگهداری نام نقش جدید
        public string Description { get; set; }
        // خاصیت Description برای نگهداری توضیحات نقش جدید
    }
}
