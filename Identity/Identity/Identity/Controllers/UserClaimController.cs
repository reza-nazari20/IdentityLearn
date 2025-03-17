using Identity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Identity.Controllers
{
    // کنترلر مدیریت کلیم‌های کاربر
    public class UserClaimController : Controller
    {
        // تعریف متغیر خصوصی برای دسترسی به مدیریت کاربران
        private readonly UserManager<User> _userManager;

        // سازنده کلاس برای تزریق وابستگی UserManager
        public UserClaimController(UserManager<User> userManager)
        {
            // مقداردهی متغیر _userManager
            _userManager = userManager;
        }

        // اکشن نمایش لیست کلیم‌های کاربر - فقط برای کاربران احراز هویت شده
        [Authorize]
        public IActionResult Index()
        {
            // بازگرداندن نما همراه با لیست کلیم‌های کاربر جاری
            return View(User.Claims);
        }

        // اکشن نمایش فرم ایجاد کلیم جدید
        [HttpGet]
        public IActionResult Create()
        {
            // بازگرداندن نمای خالی برای ایجاد کلیم جدید
            return View();
        }

        // اکشن پردازش فرم ارسالی برای ایجاد کلیم جدید
        [HttpPost]
        public IActionResult Create(string ClaimType, string ClaimValue)
        {
            // دریافت اطلاعات کاربر جاری
            var user = _userManager.GetUserAsync(User).Result;

            // ایجاد یک شیء کلیم جدید با نوع و مقدار دریافتی
            Claim newClaim = new Claim(ClaimType, ClaimValue, ClaimValueTypes.String);

            // افزودن کلیم به کاربر و منتظر نتیجه عملیات
            var result = _userManager.AddClaimAsync(user, newClaim).Result;

            // بررسی موفقیت‌آمیز بودن عملیات افزودن کلیم
            if (result.Succeeded)
            {
                // هدایت کاربر به صفحه نمایش لیست کلیم‌ها
                return RedirectToAction("Index");
            }
            else
            {
                // حلقه روی تمام خطاهای رخ داده
                foreach (var item in result.Errors)
                {
                    // افزودن خطاها به ModelState برای نمایش در نما
                    ModelState.AddModelError("", item.Description);
                }
            }

            // بازگرداندن نمای ایجاد کلیم در صورت بروز خطا
            return View();
        }

        // اکشن حذف کلیم کاربر بر اساس نوع کلیم
        public IActionResult Delete(string ClaimTypes)
        {
            // دریافت اطلاعات کاربر جاری
            var user = _userManager.GetUserAsync(User).Result;

            // جستجو و پیدا کردن کلیم مورد نظر برای حذف
            Claim claim = User.Claims.Where(p => p.Type == ClaimTypes).FirstOrDefault();

            // بررسی وجود کلیم مورد نظر
            if (claim != null)
            {
                // حذف کلیم از کاربر و منتظر نتیجه عملیات
                var result = _userManager.RemoveClaimAsync(user, claim).Result;

                // بررسی موفقیت‌آمیز بودن عملیات حذف کلیم
                if (result.Succeeded)
                {
                    // هدایت کاربر به صفحه نمایش لیست کلیم‌ها
                    return RedirectToAction("Index");
                }
            }

            // هدایت کاربر به صفحه نمایش لیست کلیم‌ها (در صورت عدم وجود کلیم یا خطا)
            return RedirectToAction("Index");
        }
    }
}
