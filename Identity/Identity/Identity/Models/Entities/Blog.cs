namespace Identity.Models.Entities
{
    // کلاس مدل برای نمایش یک بلاگ در پایگاه داده
    public class Blog
    {
        // شناسه یکتای بلاگ
        // این فیلد به عنوان کلید اصلی در پایگاه داده استفاده می‌شود
        public long Id { get; set; }

        // عنوان بلاگ
        // معمولاً برای نمایش و جستجو استفاده می‌شود
        public string Title { get; set; }

        // متن اصلی بلاگ
        // شامل محتوای اصلی نوشته می‌باشد
        public string Body { get; set; }

        // ارتباط با موجودیت کاربر
        // نشان‌دهنده کاربری که این بلاگ را ایجاد کرده است
        public User User { get; set; }

        // شناسه کاربر
        // کلید خارجی برای ارتباط با جدول کاربران
        public string UserId { get; set; }
    }
}
