using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Models.Dto.Blog
{
    // کلاس DTO (Data Transfer Object) برای انتقال اطلاعات بلاگ بین لایه‌های مختلف برنامه
    public class BlogDto
    {
        // شناسه یکتای بلاگ
        public long Id { get; set; }

        // عنوان بلاگ
        public string Title { get; set; }

        // متن اصلی بلاگ
        public string Body { get; set; }

        // شناسه کاربر ایجادکننده بلاگ
        // با ویژگی BindNever از مدل بایندینگ خارج می‌شود
        // یعنی در زمان دریافت اطلاعات از فرم، این فیلد نادیده گرفته می‌شود
        [BindNever]
        public string UserId { get; set; }

        // نام کاربری نویسنده بلاگ
        public string UserName { get; set; }
    }
}
