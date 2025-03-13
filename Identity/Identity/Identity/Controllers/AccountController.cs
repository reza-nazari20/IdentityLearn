using Identity.Models.Dto;
using Identity.Models.Dto.Account;
using Identity.Models.Entities;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        /// ///////////////////////////////////////////////////////////////شروع بخش پنل اطلاعات کاربر 🔵👇

        // این اکشن نیاز به احراز هویت دارد و فقط کاربران وارد شده می‌توانند به آن دسترسی داشته باشند
        [Authorize]
        public IActionResult Index()
        {
            // پیدا کردن کاربر فعلی با استفاده از نام کاربری ذخیره شده در هویت کاربر و منتظر نتیجه
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            // ایجاد یک نمونه جدید از مدل اطلاعات حساب کاربری و پر کردن آن با اطلاعات کاربر
            MyAccountInfoDto myAccount = new MyAccountInfoDto()
            {
                // انتساب شناسه کاربر به مدل
                Id = user.Id,

                // ترکیب نام و نام خانوادگی کاربر به عنوان نام کامل
                FullName = $"{user.FirstName}{user.LastName}",

                // انتساب نام کاربری به مدل
                UserName = user.UserName,

                // انتساب ایمیل کاربر به مدل
                Email = user.Email,

                // انتساب شماره تلفن کاربر به مدل
                PhoneNumber = user.PhoneNumber,

                // انتساب وضعیت تایید ایمیل کاربر به مدل
                EmailConfirmed = user.EmailConfirmed,

                // انتساب وضعیت تایید شماره تلفن کاربر به مدل
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,

                // انتساب وضعیت فعال بودن احراز هویت دو عاملی کاربر به مدل
                TowFactorEnable = user.TwoFactorEnabled,
            };

            // بازگرداندن نما همراه با مدل اطلاعات حساب کاربری برای نمایش
            return View(myAccount);
        }

        /// ///////////////////////////////////////////////////////////////پایان بخش پنل اطلاعات کاربر 🔵👆

        /// ///////////////////////////////////////////////////////////////شروع بخش تعیین ورود دو مرحله ای و یا برداشتن ورود دو مرحله ای 🔵👇

        // این اکشن نیاز به احراز هویت دارد و فقط کاربران وارد شده می‌توانند به آن دسترسی داشته باشند
        [Authorize]
        public IActionResult TowFactorEnable()
        {
            // پیدا کردن کاربر فعلی با استفاده از نام کاربری ذخیره شده در هویت کاربر و منتظر نتیجه
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            // تغییر وضعیت احراز هویت دو عاملی کاربر به حالت معکوس فعلی (اگر فعال بود غیرفعال می‌شود و برعکس) و منتظر نتیجه
            var result = _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled).Result;

            // هدایت کاربر به اکشن Index برای مشاهده تغییرات اعمال شده
            return RedirectToAction(nameof(Index));
        }

        /// ///////////////////////////////////////////////////////////////پایان بخش تعیین ورود دو مرحله ای و یا برداشتن ورود دو مرحله ای 🔵👆


        /// ///////////////////////////////////////////////////////////////شروع بخش ثبت نام و تایید کردن ایمیل کاربر با ارسال ایمیل تایید🔵👇

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

        /// ///////////////////////////////////////////////////////////////پایان بخش ثبت نام و تایید کردن ایمیل کاربر با ارسال ایمیل تایید🔵👆

        /// ///////////////////////////////////////////////////////////////شروع بخش ورود و خروج🔵👇
        
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

            // بررسی فعال بودن ورود دو مرحله ای
            if (result.RequiresTwoFactor == true)
            {
                // هدایت کاربر به اکشن مربوط به احراز هویت دو مرحله ای و ارسال نام کاربری و وضعیت ماندگاری ورود
                return RedirectToAction("TwoFactorLogin", new { login.UserName, login.IsPersistent });
            }

            // بازگرداندن نمای پیش‌فرض بدون مدل در صورت عدم موفقیت در ورود
            return View();
        }

        /// ///////////////////////////////////////////////////////////////شروع بخش ساخت و ارسال کد تایید دو مرحله ای از طریق پیام و یا ایمیل 🔵👇

        // متد برای دریافت درخواست احراز هویت دو مرحله‌ای و ارسال کد تایید به کاربر
        public IActionResult TwoFactorLogin(string UserName, bool IsPersistent)
        {
            // پیدا کردن کاربر با نام کاربری دریافت شده
            var user = _userManager.FindByNameAsync(UserName).Result;

            // اگر کاربری با این نام کاربری پیدا نشود، درخواست نامعتبر است
            if (user == null)
            {
                return BadRequest();
            }

            // دریافت لیست روش‌های معتبر احراز هویت دو مرحله‌ای برای کاربر
            var providers = _userManager.GetValidTwoFactorProvidersAsync(user).Result;

            // ایجاد مدل مورد نیاز برای نمایش در صفحه احراز هویت دو مرحله‌ای
            TwoFactorLoginDto model = new TwoFactorLoginDto();

            // اگر روش تأیید از طریق تلفن موجود باشد
            if (providers.Contains("Phone"))
            {
                // تولید کد تأیید برای ارسال از طریق پیامک
                string smsCode = _userManager.GenerateTwoFactorTokenAsync(user, "Phone").Result;

                // ایجاد سرویس پیامک
                SmsService smsService = new SmsService();

                // ارسال کد تأیید به شماره تلفن کاربر
                smsService.Send(user.PhoneNumber, smsCode);

                // تنظیم روش تأیید در مدل به "Phone"
                model.providers = "Phone";

                // تنظیم وضعیت ماندگاری ورود در مدل
                model.IsPersistent = IsPersistent;
            }
            // اگر روش تأیید از طریق ایمیل موجود باشد
            else if (providers.Contains("Email"))
            {
                // تولید کد تأیید برای ارسال از طریق ایمیل
                string emailCode = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;

                // ایجاد سرویس ایمیل
                EmailService emailService = new EmailService();

                // ارسال کد تأیید به ایمیل کاربر
                emailService.Execute(user.Email, $"کد ورود دو مرحله ای : {emailCode}", "ورود دو مرحله ای");

                // تنظیم روش تأیید در مدل به "Email"
                model.providers = "Email";

                // تنظیم وضعیت ماندگاری ورود در مدل
                model.IsPersistent = IsPersistent;
            }

            // بازگرداندن نما با مدل برای وارد کردن کد تأیید توسط کاربر
            return View(model);
        }

        // متد برای پردازش درخواست POST احراز هویت دو مرحله‌ای پس از وارد کردن کد تأیید توسط کاربر
        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorLoginDto twoFactor)
        {
            // بررسی اعتبارسنجی مدل - اگر مدل معتبر نباشد
            if (!ModelState.IsValid)
            {
                // بازگرداندن نما با همان مدل دریافتی برای نمایش خطاها
                return View(twoFactor);
            }

            // دریافت کاربری که در مرحله اول احراز هویت وارد شده است
            var user = _signInManager.GetTwoFactorAuthenticationUserAsync().Result;

            // اگر کاربری وجود نداشته باشد، درخواست نامعتبر است
            if (user == null)
            {
                return BadRequest();
            }

            // تلاش برای ورود کاربر با استفاده از کد تأیید دو مرحله‌ای
            var result = _signInManager.TwoFactorSignInAsync(twoFactor.providers, twoFactor.Code, twoFactor.IsPersistent, false).Result;

            // اگر ورود موفقیت‌آمیز باشد
            if (result.Succeeded)
            {
                // هدایت کاربر به صفحه اصلی
                return RedirectToAction("Index");
            }
            // اگر حساب کاربری قفل شده باشد
            else if (result.IsLockedOut)
            {
                // افزودن پیام خطا به ModelState
                ModelState.AddModelError("", "حساب کاربری شما قفل شده است");

                // بازگرداندن نما بدون مدل
                return View();
            }
            // در صورت عدم موفقیت دیگر (کد نادرست)
            else
            {
                // افزودن پیام خطا به ModelState
                ModelState.AddModelError("", "کد وارد شده صحیح نیست ");

                // بازگرداندن نما بدون مدل
                return View();
            }
        }

        /// ///////////////////////////////////////////////////////////////پایان بخش ساخت و ارسال کد تایید دو مرحله ای از طریق پیام و یا ایمیل 🔵👆

        // متد اکشن LogOut برای خروج کاربر از سیستم
        public IActionResult LogOut()
        {
            // خروج کاربر فعلی از سیستم با استفاده از _signInManager
            _signInManager.SignOutAsync();

            // هدایت کاربر به اکشن Index از کنترلر Home
            return RedirectToAction("Index", "Home");
        }

        /// ///////////////////////////////////////////////////////////////پایان بخش ورود و خروج🔵👆

        /// ///////////////////////////////////////////////////////////////شروع بخش فراموشی رمز عبور و تغییر رمز عبور با ارسال ایمیل تایید به ایمیل کاربر🔵👇

        // اکشن برای نمایش فرم فراموشی رمز عبور
        public IActionResult ForgotPassword()
        {
            return View(); // بازگرداندن ویو مربوط به فراموشی رمز عبور
        }

        // اکشن برای پردازش درخواست فراموشی رمز عبور
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordConfirmationDto forgot)
        {
            // بررسی صحت اطلاعات ورودی
            if (!ModelState.IsValid)
            {
                return View(forgot); // در صورت نامعتبر بودن داده‌ها، فرم همراه با اطلاعات ارسال‌شده مجدداً نمایش داده می‌شود
            }

            // جستجوی کاربر بر اساس ایمیل وارد شده
            var user = _userManager.FindByEmailAsync(forgot.Email).Result;

            // بررسی اینکه آیا کاربر وجود دارد و ایمیل وی تأیید شده است
            if (user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                // نمایش پیام خطا در صورت عدم اعتبار ایمیل یا تأیید نشدن آن
                ViewBag.message = "ممکن است ایمیل وارد شده معتبر نباشد! و یا اینکه ایمیل خود را تایید نکرده باشید";
                return View(); // بازگرداندن فرم با پیام خطا
            }

            // تولید توکن برای تنظیم مجدد رمز عبور
            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            // ساخت لینک بازنشانی رمز عبور با استفاده از Url.Action
            string callbakUrl = Url.Action("ResetPassword", "Account", new
            {
                UserId = user.Id, // ارسال شناسه کاربر
                token = token // ارسال توکن تولید شده
            }, protocol: Request.Scheme); // تعیین پروتکل درخواست (http یا https) به‌صورت خودکار بر اساس تنظیمات سرور

            // تولید متن ایمیل حاوی لینک بازنشانی رمز عبور
            string body = $"برای تنظیم مجدد رمز عبور بر روی لینک زیر کلیک کنید <br/> <a href={callbakUrl}> LINK RESET PASSWORD  </a>";

            // ارسال ایمیل به آدرس کاربر با استفاده از سرویس ایمیل
            _emailService.Execute(user.Email, body, "فراموشی رمز عبور");

            // ذخیره پیام موفقیت در ViewBag برای نمایش در صفحه
            ViewBag.message = "لینک تنظیم مجدد رمز عبور برای ایمیل شما ارسال شد";

            return View(); // بازگرداندن فرم همراه با پیام موفقیت
        }

        // اکشن GET برای نمایش فرم تنظیم مجدد رمز عبور
        public IActionResult ResetPassword(string UserId, string Token)
        {
            return View(new ResetPasswordDto
            {
                Token = Token, // مقداردهی توکن بازنشانی رمز عبور
                UserId = UserId // مقداردهی شناسه کاربر
            });
        }

        // اکشن برای پردازش درخواست تنظیم مجدد رمز عبور
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto reset)
        {
            // بررسی صحت اطلاعات ورودی
            if (!ModelState.IsValid)
            {
                return View(reset); // در صورت نامعتبر بودن داده‌ها، فرم همراه با اطلاعات ارسال‌شده مجدداً نمایش داده می‌شود
            }

            // بررسی اینکه رمز عبور و تأیید آن یکسان هستند
            if (reset.Password != reset.ConfirmPassword)
            {
                return BadRequest(); // در صورت عدم تطابق، درخواست نامعتبر اعلام می‌شود
            }

            // جستجوی کاربر بر اساس شناسه وارد شده
            var user = _userManager.FindByIdAsync(reset.UserId).Result;

            // بررسی اینکه آیا کاربر وجود دارد
            if (user == null)
            {
                return BadRequest(); // اگر کاربر یافت نشد، درخواست نامعتبر اعلام می‌شود
            }

            // بازنشانی رمز عبور کاربر با استفاده از توکن و رمز جدید
            var Result = _userManager.ResetPasswordAsync(user, reset.Token, reset.Password).Result;

            // بررسی موفقیت‌آمیز بودن عملیات تغییر رمز عبور
            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation)); // هدایت به صفحه تأیید بازنشانی رمز عبور
            }
            else
            {
                // در صورت ناموفق بودن عملیات، نمایش پیام‌های خطای مربوطه
                ViewBag.Errors = Result.Errors;
                return View(reset); // بازگرداندن فرم همراه با خطاها
            }
        }

        // اکشن برای نمایش صفحه تأیید بازنشانی رمز عبور
        public IActionResult ResetPasswordConfirmation()
        {
            return View(); // بازگرداندن ویو تأیید موفقیت‌آمیز بودن فرآیند تغییر رمز عبور
        }

        /// ///////////////////////////////////////////////////////////////پایان بخش فراموشی رمز عبور و تغییر رمز عبور با ارسال ایمیل تایید به ایمیل کاربر🔵👆

        [Authorize]
        // اعمال فیلتر احراز هویت - فقط کاربران لاگین شده می‌توانند به این اکشن دسترسی داشته باشند
        public IActionResult SetPhoneNumber()
        {
            // بازگرداندن نمای مربوط به تنظیم شماره تلفن
            return View();
        }

        [HttpPost]
        [Authorize]
        // اعمال فیلتر احراز هویت - فقط کاربران لاگین شده می‌توانند به این اکشن دسترسی داشته باشند
        public IActionResult SetPhoneNumber(SetPhoneNumberDto phoneNumberDto)
        {
            // یافتن کاربر فعلی با استفاده از نام کاربری و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            // تنظیم شماره تلفن برای کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var setResult = _userManager.SetPhoneNumberAsync(user, phoneNumberDto.PhoneNumber).Result;

            // تولید کد تایید برای شماره تلفن جدید و منتظر نتیجه با استفاده از Result
            string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumberDto.PhoneNumber).Result;

            // ایجاد نمونه‌ای از سرویس ارسال پیامک
            SmsService smsService = new SmsService();

            // ارسال کد تایید به شماره تلفن وارد شده
            smsService.Send(phoneNumberDto.PhoneNumber, code);

            // ذخیره شماره تلفن در TempData برای استفاده در اکشن بعدی
            TempData["PhoneNumber"] = phoneNumberDto.PhoneNumber;

            // هدایت کاربر به اکشن تایید شماره تلفن
            return RedirectToAction(nameof(VerifyPhoneNumber));
        }

        [Authorize]
        // اعمال فیلتر احراز هویت - فقط کاربران لاگین شده می‌توانند به این اکشن دسترسی داشته باشند
        public IActionResult VerifyPhoneNumber()
        {
            // بازگرداندن نمای تایید شماره تلفن با مدل VerifyPhoneNumberDto که شماره تلفن از TempData در آن قرار گرفته است
            return View(new VerifyPhoneNumberDto
            {
                // دریافت شماره تلفن ذخیره شده در TempData و تبدیل آن به رشته
                PhoneNumber = TempData["PhoneNumber"].ToString(),
            });
        }

        [HttpPost]
        [Authorize]
        // اعمال فیلتر احراز هویت - فقط کاربران لاگین شده می‌توانند به این اکشن دسترسی داشته باشند
        public IActionResult VerifyPhoneNumber(VerifyPhoneNumberDto verify)
        {
            // یافتن کاربر فعلی با استفاده از نام کاربری و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            // بررسی صحت کد وارد شده برای تایید شماره تلفن و منتظر نتیجه با استفاده از Result
            bool resultVerify = _userManager.VerifyChangePhoneNumberTokenAsync(user, verify.Code, verify.PhoneNumber).Result;

            // اگر کد وارد شده صحیح نباشد
            if (resultVerify == false)
            {
                // نمایش پیام خطا به کاربر با استفاده از ViewData
                ViewData["Message"] = $"کد وارد شده برای شماره {verify.PhoneNumber}اشتباه است";

                // بازگرداندن نما با همان مدل برای اصلاح توسط کاربر
                return View(verify);
            }
            else
            {
                // تایید شماره تلفن کاربر در صورت صحت کد
                user.PhoneNumberConfirmed = true;

                // به‌روزرسانی اطلاعات کاربر در دیتابیس
                _userManager.UpdateAsync(user);
            }

            // هدایت کاربر به صفحه موفقیت‌آمیز بودن عملیات تایید
            return RedirectToAction("VerifySuccess");
        }

        public IActionResult VerifySuccess()
        {
            // بازگرداندن نمای موفقیت‌آمیز بودن عملیات تایید شماره تلفن
            return View();
        }
    }
}
