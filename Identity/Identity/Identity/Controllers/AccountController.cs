using Identity.Models.Dto;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Identity.Controllers
{
    public class AccountController : Controller
    // کلاس کنترلر مدیریت حساب کاربری
    {
        private readonly UserManager<User> _userManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت کاربران
        private readonly SignInManager<User> _signInManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت ورود و خروج کاربران

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        // سازنده کلاس که دو پارامتر برای مدیریت کاربران و مدیریت ورود و خروج دریافت می‌کند (تزریق وابستگی)
        {
            _userManager = userManager;
            // مقداردهی فیلد _userManager با مقدار دریافتی از پارامتر
            _signInManager = signInManager;
            // مقداردهی فیلد _signInManager با مقدار دریافتی از پارامتر
        }

        public IActionResult Index()
        // متد اکشن Index برای نمایش صفحه اصلی حساب کاربری
        {
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل
        }

        public IActionResult Register()
        // متد اکشن Register برای نمایش فرم ثبت‌نام کاربر جدید
        {
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Register(RegisterDto register)
        // متد اکشن Register برای پردازش فرم ارسالی ثبت‌نام کاربر جدید
        {
            if (ModelState.IsValid == false)
            // بررسی اعتبارسنجی مدل - اگر مدل معتبر نباشد
            {
                return View(register);
                // بازگرداندن نما با همان مدل دریافتی برای نمایش خطاها
            }

            User newUser = new User()
            // ایجاد یک شیء جدید از کلاس User
            {
                FirstName = register.FirstName,
                // انتساب نام از مدل دریافتی به شیء کاربر جدید
                LastName = register.LastName,
                // انتساب نام خانوادگی از مدل دریافتی به شیء کاربر جدید
                Email = register.Email,
                // انتساب ایمیل از مدل دریافتی به شیء کاربر جدید
                UserName = register.Email,
                // استفاده از ایمیل به عنوان نام کاربری
            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;
            // ایجاد کاربر جدید با استفاده از _userManager و منتظر نتیجه با استفاده از Result

            if (result.Succeeded)
            // بررسی موفقیت‌آمیز بودن عملیات ایجاد کاربر
            {
                return RedirectToAction("Index", "Home");
                // هدایت کاربر به اکشن Index از کنترلر Home
            }

            string message = "";
            // ایجاد یک رشته خالی برای نگهداری پیام‌های خطا
            foreach (var item in result.Errors.ToList())
            // حلقه روی تمام خطاهای رخ داده
            {
                message += item.Description + Environment.NewLine;
                // اضافه کردن توضیح خطا به متغیر message و افزودن خط جدید
            }
            TempData["Message"] = message;
            // ذخیره پیام‌های خطا در TempData برای نمایش در نما
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل
        }

        [HttpGet]
        // مشخص می‌کند که این متد فقط به درخواست‌های GET پاسخ می‌دهد
        public IActionResult Login(string returnurl = "/")
        // متد اکشن Login برای نمایش فرم ورود به سیستم با پارامتر اختیاری مسیر بازگشت
        {
            return View(new LoginDto
            // ایجاد یک شیء جدید از کلاس LoginDto و ارسال آن به نما
            {
                ReturnUrl = returnurl,
                // انتساب مسیر بازگشت دریافتی به ReturnUrl در LoginDto
            });
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Login(LoginDto login)
        // متد اکشن Login برای پردازش فرم ارسالی ورود به سیستم
        {
            if (!ModelState.IsValid)
            // بررسی اعتبارسنجی مدل - اگر مدل معتبر نباشد
            {
                return View(login);
                // بازگرداندن نما با همان مدل دریافتی برای نمایش خطاها
            }

            var user = _userManager.FindByNameAsync(login.UserName).Result;
            // پیدا کردن کاربر با نام کاربری مشخص شده و منتظر نتیجه با استفاده از Result

            _signInManager.SignOutAsync();
            // خروج کاربر فعلی از سیستم قبل از ورود کاربر جدید

            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent
                , true).Result;
            // ورود کاربر به سیستم با استفاده از _signInManager و منتظر نتیجه با استفاده از Result
            // پارامتر سوم (login.IsPersistent) مشخص می‌کند آیا اطلاعات ورود ذخیره شود یا خیر (به خاطر سپردن ورود)
            // پارامتر چهارم (true) امکان قفل شدن حساب کاربری در صورت ورود ناموفق متعدد را فعال می‌کند

            if (result.Succeeded == true)
            // بررسی موفقیت‌آمیز بودن عملیات ورود به سیستم
            {
                return Redirect(login.ReturnUrl);
                // هدایت کاربر به مسیر بازگشت مشخص شده
            }
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل در صورت عدم موفقیت در ورود
        }

        public IActionResult LogOut()
        // متد اکشن LogOut برای خروج کاربر از سیستم
        {
            _signInManager.SignOutAsync();
            // خروج کاربر فعلی از سیستم با استفاده از _signInManager
            return RedirectToAction("Index", "Home");
            // هدایت کاربر به اکشن Index از کنترلر Home
        }
    }
}
