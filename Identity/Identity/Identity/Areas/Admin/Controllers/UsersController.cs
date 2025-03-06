using Identity.Areas.Admin.Data.Dto;
using Identity.Areas.Admin.Data.Dto.Roles;
using Identity.Models.Dto;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    // این خط مشخص می‌کند که این کنترلر در ناحیه Admin قرار دارد
    public class UsersController : Controller
    // تعریف کلاس UsersController که از کلاس Controller ارث‌بری می‌کند
    {
        private readonly UserManager<User> _userManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت کاربران
        private readonly RoleManager<Role> _roleManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت نقش‌ها

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        // سازنده کلاس که دو پارامتر برای مدیریت کاربران و نقش‌ها دریافت می‌کند (تزریق وابستگی)
        {
            _userManager = userManager;
            // مقداردهی فیلد _userManager با مقدار دریافتی از پارامتر
            _roleManager = roleManager;
            // مقداردهی فیلد _roleManager با مقدار دریافتی از پارامتر
        }

        public IActionResult Index()
        // متد اکشن Index که لیست کاربران را نمایش می‌دهد
        {
            var users = _userManager.Users
                // دریافت لیست تمام کاربران از _userManager
                .Select(p => new UserListDto
                // تبدیل هر کاربر به یک شیء UserListDto با استفاده از LINQ
                {
                    Id = p.Id,
                    // انتساب شناسه کاربر به Id در UserListDto
                    FirstName = p.FirstName,
                    // انتساب نام کاربر به FirstName در UserListDto
                    LastName = p.LastName,
                    // انتساب نام خانوادگی کاربر به LastName در UserListDto
                    UserName = p.UserName,
                    // انتساب نام کاربری به UserName در UserListDto
                    PhoneNumber = p.PhoneNumber,
                    // انتساب شماره تلفن کاربر به PhoneNumber در UserListDto
                    EmailConfirmed = p.EmailConfirmed,
                    // انتساب وضعیت تأیید ایمیل به EmailConfirmed در UserListDto
                    AccessFailedCount = p.AccessFailedCount
                    // انتساب تعداد دفعات ورود ناموفق به AccessFailedCount در UserListDto
                }).ToList();
            // تبدیل نتیجه به یک لیست
            return View(users);
            // ارسال لیست کاربران به نمای مربوطه
        }

        public IActionResult Create()
        // متد اکشن Create برای نمایش فرم ایجاد کاربر جدید
        {
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Create(RegisterDto register)
        // متد اکشن Create برای پردازش فرم ارسالی ایجاد کاربر جدید
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
                return RedirectToAction("Index", "users", new { area = "admin" });
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
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
            return View(register);
            // بازگرداندن نما با همان مدل دریافتی
        }

        public IActionResult Edit(string Id)
        // متد اکشن Edit برای نمایش فرم ویرایش کاربر
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result

            UserEditDto userEdit = new UserEditDto()
            // ایجاد یک شیء جدید از کلاس UserEditDto
            {
                Id = user.Id,
                // انتساب شناسه کاربر به Id در UserEditDto
                FirstName = user.FirstName,
                // انتساب نام کاربر به FirstName در UserEditDto
                LastName = user.LastName,
                // انتساب نام خانوادگی کاربر به LastName در UserEditDto
                Email = user.Email,
                // انتساب ایمیل کاربر به Email در UserEditDto
                UserName = user.UserName,
                // انتساب نام کاربری به UserName در UserEditDto
                PhoneNumber = user.PhoneNumber,
                // انتساب شماره تلفن کاربر به PhoneNumber در UserEditDto
            };

            return View(userEdit);
            // ارسال مدل به نمای مربوطه
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Edit(UserEditDto userEdit)
        // متد اکشن Edit برای پردازش فرم ویرایش کاربر
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result
            user.FirstName = userEdit.FirstName;
            // به‌روزرسانی نام کاربر با مقدار جدید از مدل
            user.LastName = userEdit.LastName;
            // به‌روزرسانی نام خانوادگی کاربر با مقدار جدید از مدل
            user.Email = userEdit.Email;
            // به‌روزرسانی ایمیل کاربر با مقدار جدید از مدل
            user.UserName = userEdit.UserName;
            // به‌روزرسانی نام کاربری با مقدار جدید از مدل
            user.PhoneNumber = userEdit.PhoneNumber;
            // به‌روزرسانی شماره تلفن کاربر با مقدار جدید از مدل

            var result = _userManager.UpdateAsync(user).Result;
            // به‌روزرسانی کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result

            if (result.Succeeded)
            // بررسی موفقیت‌آمیز بودن عملیات به‌روزرسانی
            {
                return RedirectToAction("Index", "users", new { area = "admin" });
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
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
            return View(userEdit);
            // بازگرداندن نما با همان مدل دریافتی
        }

        public IActionResult Delete(string Id)
        // متد اکشن Delete برای نمایش صفحه تأیید حذف کاربر
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            UserDeleteDto userDelete = new UserDeleteDto()
            // ایجاد یک شیء جدید از کلاس UserDeleteDto
            {
                Id = user.Id,
                // انتساب شناسه کاربر به Id در UserDeleteDto
                FullName = $"{user.FirstName}{user.LastName}",
                // ترکیب نام و نام خانوادگی کاربر و انتساب به FullName در UserDeleteDto
                Email = user.Email,
                // انتساب ایمیل کاربر به Email در UserDeleteDto
                UserName = user.UserName,
                // انتساب نام کاربری به UserName در UserDeleteDto
            };
            return View(userDelete);
            // ارسال مدل به نمای مربوطه
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Delete(UserDeleteDto userDelete)
        // متد اکشن Delete برای پردازش درخواست حذف کاربر
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result

            var result = _userManager.DeleteAsync(user).Result;
            // حذف کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result

            if (result.Succeeded)
            // بررسی موفقیت‌آمیز بودن عملیات حذف
            {
                return RedirectToAction("Index", "users", new { area = "admin" });
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
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
            return View(userDelete);
            // بازگرداندن نما با همان مدل دریافتی
        }

        public IActionResult AddUserRole(string Id)
        // متد اکشن AddUserRole برای نمایش فرم افزودن نقش به کاربر
        {

            var user = _userManager.FindByIdAsync(Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result

            var roles = new List<SelectListItem>(
                // ایجاد یک لیست از SelectListItem‌ها برای نمایش در دراپ‌داون نقش‌ها
                _roleManager.Roles.Select(p => new SelectListItem
                // تبدیل هر نقش به یک SelectListItem با استفاده از LINQ
                {
                    Text = p.Name,
                    // انتساب نام نقش به Text در SelectListItem (متنی که نمایش داده می‌شود)
                    Value = p.Name,
                    // انتساب نام نقش به Value در SelectListItem (مقداری که هنگام انتخاب ارسال می‌شود)
                }
                ).ToList());
            // تبدیل نتیجه به یک لیست

            return View(new AddUserRoleDto
            // ایجاد یک شیء جدید از کلاس AddUserRoleDto و ارسال آن به نما
            {
                Id = Id,
                // انتساب شناسه کاربر به Id در AddUserRoleDto
                Roles = roles,
                // انتساب لیست نقش‌ها به Roles در AddUserRoleDto
                Email = user.Email,
                // انتساب ایمیل کاربر به Email در AddUserRoleDto
                FullName = $"{user.FirstName}  {user.LastName}"
                // ترکیب نام و نام خانوادگی کاربر و انتساب به FullName در AddUserRoleDto
            });
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult AddUserRole(AddUserRoleDto newRole)
        // متد اکشن AddUserRole برای پردازش درخواست افزودن نقش به کاربر
        {
            var user = _userManager.FindByIdAsync(newRole.Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result
            var result = _userManager.AddToRoleAsync(user, newRole.Role).Result;
            // افزودن نقش به کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            return RedirectToAction("UserRoles", "Users", new { Id = user.Id, area = "admin" });
            // هدایت کاربر به اکشن UserRoles از کنترلر Users در ناحیه admin با ارسال Id کاربر
        }

        public IActionResult UserRoles(string Id)
        // متد اکشن UserRoles برای نمایش لیست نقش‌های یک کاربر
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            var roles = _userManager.GetRolesAsync(user).Result;
            // دریافت لیست نقش‌های کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            ViewBag.UserInfo = $"Name : {user.FirstName} {user.LastName} Email:{user.Email}";
            // ذخیره اطلاعات کاربر در ViewBag برای نمایش در نما
            return View(roles);
            // ارسال لیست نقش‌ها به نمای مربوطه
        }
    }
}
