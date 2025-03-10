using Identity.Models.Dto;
using Identity.Models.Entities;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;


        // سازنده کلاس که دو پارامتر برای مدیریت کاربران و مدیریت ورود و خروج دریافت می‌کند (تزریق وابستگی)
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = new EmailService();
        }

        // متد اکشن Index برای نمایش صفحه اصلی حساب کاربری
        public IActionResult Index()
        {
            // بازگرداندن نمای پیش‌فرض بدون مدل
            return View();
        }

        // متد اکشن Register برای نمایش فرم ثبت‌نام کاربر جدید
        public IActionResult Register()
        {
            // بازگرداندن نمای پیش‌فرض بدون مدل
            return View();
        }

        [HttpPost]
        // متد اکشن Register برای پردازش فرم ارسالی ثبت‌نام کاربر جدید
        public IActionResult Register(RegisterDto register)
        {
            // بررسی اعتبارسنجی مدل - اگر مدل معتبر نباشد
            if (ModelState.IsValid == false)
            {
                // بازگرداندن نما با همان مدل دریافتی برای نمایش خطاها
                return View(register);
            }

            // ایجاد یک شیء جدید از کلاس User
            User newUser = new User()
            {
                // انتساب اطلاعات از مدل دریافتی به شیء کاربر جدید
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
            };

            // ایجاد کاربر جدید با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var result = _userManager.CreateAsync(newUser, register.Password).Result;

            // بررسی موفقیت‌آمیز بودن عملیات ایجاد کاربر
            if (result.Succeeded)
            {
                // تولید توکن تایید ایمیل برای کاربر جدید
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;

                // ایجاد URL فعال‌سازی حساب کاربری (آدرس بازگشتی) با استفاده از متد Url.Action
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    // ارسال شناسه کاربر به عنوان پارامتر
                    UserId = newUser.Id,
                    // ارسال توکن تایید به عنوان پارامتر
                    token = token
                }, protocol: Request.Scheme);

                // تعریف متن پیام ایمیل شامل لینک فعال‌سازی
                string body = $"لطفا برای فعال سازی حساب کاربری بر روی لینک زیر کلیک کنید <br/> <a href={callbackUrl}> LINK </a>";

                // ارسال ایمیل با استفاده از سرویس ایمیل به آدرس ایمیل کاربر
                _emailService.Execute(newUser.Email, body, "فعال سازی حساب کاربری");

                // هدایت کاربر به صفحه نمایش پیام ارسال ایمیل
                return RedirectToAction("DisplayEmail");
            }

            // ایجاد یک رشته خالی برای نگهداری پیام‌های خطا
            string message = "";

            // حلقه روی تمام خطاهای رخ داده
            foreach (var item in result.Errors.ToList())
            {
                // اضافه کردن توضیح خطا به متغیر message و افزودن خط جدید
                message += item.Description + Environment.NewLine;
            }

            // ذخیره پیام‌های خطا در TempData برای نمایش در نما
            TempData["Message"] = message;

            // بازگرداندن نمای پیش‌فرض بدون مدل
            return View();
        }


        // متد تایید ایمیل که از طریق لینک ارسال شده به ایمیل کاربر فراخوانی می‌شود
        public IActionResult ConfirmEmail(string UserId, string Token)
        {
            // بررسی معتبر بودن پارامترهای دریافتی - اگر شناسه کاربر یا توکن خالی باشد
            if (UserId == null || Token == null)
            {
                // بازگرداندن پاسخ خطای درخواست نامعتبر (400)
                return BadRequest();
            }

            // جستجوی کاربر بر اساس شناسه دریافتی
            var user = _userManager.FindByIdAsync(UserId).Result;

            // بررسی وجود کاربر - اگر کاربر یافت نشد
            if (user == null)
            {
                // نمایش صفحه خطا به کاربر
                return View("Error");
            }

            // تایید ایمیل کاربر با استفاده از توکن دریافتی
            var result = _userManager.ConfirmEmailAsync(user, Token).Result;

            // هدایت کاربر به صفحه ورود پس از تایید موفقیت‌آمیز ایمیل
            return RedirectToAction("Login");
        }

        /// <summary>
        /// نمایش صفحه تایید ایمیل
        /// </summary>
        public IActionResult DisplayEmail()
        {
            return View();
        }

        [HttpGet]
        // متد اکشن Login برای نمایش فرم ورود به سیستم با پارامتر اختیاری مسیر بازگشت
        public IActionResult Login(string returnurl = "/")
        {
            // ایجاد یک شیء جدید از کلاس LoginDto و ارسال آن به نما
            return View(new LoginDto
            {
                // انتساب مسیر بازگشت دریافتی به ReturnUrl در LoginDto
                ReturnUrl = returnurl,
            });
        }

        [HttpPost]
        // متد اکشن Login برای پردازش فرم ارسالی ورود به سیستم
        public IActionResult Login(LoginDto login)
        {
            // بررسی اعتبارسنجی مدل - اگر مدل معتبر نباشد
            if (!ModelState.IsValid)
            {
                // بازگرداندن نما با همان مدل دریافتی برای نمایش خطاها
                return View(login);
            }

            // پیدا کردن کاربر با نام کاربری مشخص شده و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByNameAsync(login.UserName).Result;

            // خروج کاربر فعلی از سیستم قبل از ورود کاربر جدید
            _signInManager.SignOutAsync();

            // ورود کاربر به سیستم با استفاده از _signInManager و منتظر نتیجه با استفاده از Result
            // پارامتر سوم (login.IsPersistent) مشخص می‌کند آیا اطلاعات ورود ذخیره شود یا خیر (به خاطر سپردن ورود)
            // پارامتر چهارم (true) امکان قفل شدن حساب کاربری در صورت ورود ناموفق متعدد را فعال می‌کند
            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent
                , true).Result;

            // بررسی موفقیت‌آمیز بودن عملیات ورود به سیستم
            if (result.Succeeded == true)
            {
                // هدایت کاربر به مسیر بازگشت مشخص شده
                return Redirect(login.ReturnUrl);
            }
            // بازگرداندن نمای پیش‌فرض بدون مدل در صورت عدم موفقیت در ورود
            return View();
        }

        // متد اکشن LogOut برای خروج کاربر از سیستم
        public IActionResult LogOut()
        {
            // خروج کاربر فعلی از سیستم با استفاده از _signInManager
            _signInManager.SignOutAsync();

            // هدایت کاربر به اکشن Index از کنترلر Home
            return RedirectToAction("Index", "Home");
        }
    }
}
