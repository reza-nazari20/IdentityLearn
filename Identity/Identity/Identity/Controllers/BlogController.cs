using Identity.Data;
using Identity.Models.Dto.Blog;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata;

namespace Identity.Controllers
{
    // کنترلر بلاگ که فقط کاربران با نقش ادمین به آن دسترسی دارند
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        // تعریف یک فیلد برای کانتکست پایگاه داده با قابلیت خواندن و نوشتن
        private readonly DataBaseContext _context;

        // تعریف یک فیلد برای مدیریت کاربران و عملیات مرتبط با آنها
        private readonly UserManager<User> _userManager;

        // تعریف یک فیلد برای سرویس احراز مجوز و بررسی دسترسی‌ها
        private readonly IAuthorizationService _authorizationService;

        // سازنده کلاس برای تزریق وابستگی‌های مورد نیاز به کنترلر
        public BlogController(DataBaseContext context, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            // مقداردهی فیلد کانتکست پایگاه داده
            _context = context;

            // مقداردهی فیلد مدیریت کاربران
            _userManager = userManager;

            // مقداردهی فیلد سرویس احراز مجوز
            _authorizationService = authorizationService;
        }

        // اکشن نمایش لیست تمام بلاگ‌ها
        public IActionResult Index()
        {
            // واکشی بلاگ‌ها از پایگاه داده همراه با اطلاعات کاربر
            var blogs = _context.Blogs
                // شامل کردن اطلاعات کاربر برای هر بلاگ
                .Include(p => p.User)
                // تبدیل اطلاعات به یک مدل داده‌ای انتقالی (DTO)
                .Select(
                p => new BlogDto
                {
                    // انتساب شناسه بلاگ
                    Id = p.Id,
                    // انتساب متن بلاگ
                    Body = p.Body,
                    // انتساب عنوان بلاگ
                    Title = p.Title,
                    // انتساب نام کاربری نویسنده بلاگ
                    UserName = p.User.UserName,
                });
            // ارسال لیست بلاگ‌ها به View برای نمایش
            return View(blogs);
        }

        // اکشن نمایش فرم ایجاد بلاگ جدید
        public IActionResult Create()
        {
            // نمایش View برای ایجاد بلاگ
            return View();
        }

        // اکشن ثبت بلاگ جدید با دریافت اطلاعات از فرم
        [HttpPost]
        public IActionResult Create(BlogDto blog)
        {
            // واکشی کاربر جاری که درخواست ایجاد بلاگ را داده است
            var user = _userManager.GetUserAsync(User).Result;

            // ایجاد یک موجودیت بلاگ جدید
            Blog newBlog = new Blog()
            {
                // انتساد متن بلاگ
                Body = blog.Body,
                // انتساب عنوان بلاگ
                Title = blog.Title,
                // انتساب کاربر به عنوان نویسنده بلاگ
                User = user
            };

            // اضافه کردن بلاگ جدید به مجموعه بلاگ‌ها
            _context.Add(newBlog);
            // ذخیره تغییرات در پایگاه داده
            _context.SaveChanges();

            // بازگشت به صفحه اصلی بلاگ‌ها
            return RedirectToAction("Index");
        }

        // اکشن نمایش فرم ویرایش بلاگ
        public IActionResult Edit(long Id)
        {
            // واکشی بلاگ مورد نظر همراه با اطلاعات کاربر
            // واکشی بلاگ مورد نظر از پایگاه داده با شناسه داده شده
            var blog = _context.Blogs
               // شامل کردن اطلاعات کاربر مرتبط با بلاگ (برای جلوگیری از lazy loading)
               .Include(p => p.User)
               // فیلتر کردن بلاگ‌ها برای یافتن بلاگی با شناسه مورد نظر
               .Where(p => p.Id == Id)
               // تبدیل اطلاعات بلاگ به یک مدل داده‌ای انتقالی (DTO)
               .Select(p => new BlogDto
               {
                   // انتساد شناسه بلاگ
                   Id = p.Id,
                   // انتساد عنوان بلاگ
                   Title = p.Title,
                   // انتساد متن بلاگ
                   Body = p.Body,
                   // انتساد شناسه کاربر ایجادکننده بلاگ
                   UserId = p.UserId,
                   // انتساد نام کاربری نویسنده بلاگ
                   UserName = p.User.UserName,
               })
               // دریافت اولین (و تنها) بلاگ مطابق با شرایط
               // اگر بلاگی یافت نشود، مقدار null برگردانده می‌شود
               .FirstOrDefault();

            // بررسی مجوز کاربر برای ویرایش بلاگ
            var result = _authorizationService.AuthorizeAsync(User, blog, "IsBlogForUser").Result;

            // در صورت داشتن مجوز، نمایش فرم ویرایش
            if (result.Succeeded)
            {
                return View(blog);
            }
            else
            {
                // در صورت نداشتن مجوز، هدایت به صفحه احراز هویت
                return new ChallengeResult();
            }
        }

        // اکشن ثبت تغییرات بلاگ
        [HttpPost]
        public IActionResult Edit(BlogDto blog)
        {
            // در این مثال هنوز پیاده‌سازی نشده است
            return View();
        }
    }
}
