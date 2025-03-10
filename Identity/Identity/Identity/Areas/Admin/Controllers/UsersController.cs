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
    // این خط مشخص می‌کند که این کنترلر در ناحیه Admin قرار دارد
    [Area("Admin")]
    public class UsersController : Controller
    {
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت کاربران
        private readonly UserManager<User> _userManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت نقش‌ها
        private readonly RoleManager<Role> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            // دریافت لیست تمام کاربران از _userManager
            var users = _userManager.Users
                // تبدیل هر کاربر به یک شیء UserListDto با استفاده از LINQ
                .Select(p => new UserListDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    EmailConfirmed = p.EmailConfirmed,
                    AccessFailedCount = p.AccessFailedCount
                    // تبدیل نتیجه به یک لیست
                }).ToList();
            // ارسال لیست کاربران به نمای مربوطه
            return View(users);
        }

        // متد اکشن Create برای نمایش فرم ایجاد کاربر جدید
        public IActionResult Create()
        {
            // بازگرداندن نمای پیش‌فرض بدون مدل
            return View();
        }

        [HttpPost]
        // متد اکشن Create برای پردازش فرم ارسالی ایجاد کاربر جدید
        public IActionResult Create(RegisterDto register)
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
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
                return RedirectToAction("Index", "users", new { area = "admin" });
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
            // بازگرداندن نما با همان مدل دریافتی
            return View(register);
        }

        // متد اکشن Edit برای نمایش فرم ویرایش کاربر
        public IActionResult Edit(string Id)
        {
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(Id).Result;

            // ایجاد یک شیء جدید از کلاس UserEditDto
            UserEditDto userEdit = new UserEditDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };

            // ارسال مدل به نمای مربوطه
            return View(userEdit);
        }

        [HttpPost]
        // متد اکشن Edit برای پردازش فرم ویرایش کاربر
        public IActionResult Edit(UserEditDto userEdit)
        {
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            // به‌روزرسانی اطلاعات کاربر با مقادیر جدید از مدل
            user.FirstName = userEdit.FirstName;
            user.LastName = userEdit.LastName;
            user.Email = userEdit.Email;
            user.UserName = userEdit.UserName;
            user.PhoneNumber = userEdit.PhoneNumber;

            // به‌روزرسانی کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var result = _userManager.UpdateAsync(user).Result;

            // بررسی موفقیت‌آمیز بودن عملیات به‌روزرسانی
            if (result.Succeeded)
            {
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
                return RedirectToAction("Index", "users", new { area = "admin" });
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

            // بازگرداندن نما با همان مدل دریافتی
            return View(userEdit);
        }

        // متد اکشن Delete برای نمایش صفحه تأیید حذف کاربر
        public IActionResult Delete(string Id)
        {
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(Id).Result;

            // ایجاد یک شیء جدید از کلاس UserDeleteDto
            UserDeleteDto userDelete = new UserDeleteDto()
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
            // ارسال مدل به نمای مربوطه
            return View(userDelete);
        }

        [HttpPost]
        // متد اکشن Delete برای پردازش درخواست حذف کاربر
        public IActionResult Delete(UserDeleteDto userDelete)
        {
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;

            // حذف کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var result = _userManager.DeleteAsync(user).Result;

            // بررسی موفقیت‌آمیز بودن عملیات حذف
            if (result.Succeeded)
            {
                // هدایت کاربر به اکشن Index از کنترلر users در ناحیه admin
                return RedirectToAction("Index", "users", new { area = "admin" });
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

            // بازگرداندن نما با همان مدل دریافتی
            return View(userDelete);
        }

        // متد اکشن AddUserRole برای نمایش فرم افزودن نقش به کاربر
        public IActionResult AddUserRole(string Id)
        {

            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(Id).Result;

            // ایجاد یک لیست از SelectListItem‌ها برای نمایش در دراپ‌داون نقش‌ها
            var roles = new List<SelectListItem>(
                // تبدیل هر نقش به یک SelectListItem با استفاده از LINQ
                _roleManager.Roles.Select(p => new SelectListItem
                {
                    // انتساب نام نقش به Text در SelectListItem (متنی که نمایش داده می‌شود)
                    Text = p.Name,
                    // انتساب نام نقش به Value در SelectListItem (مقداری که هنگام انتخاب ارسال می‌شود)
                    Value = p.Name,
                }
                // تبدیل نتیجه به یک لیست
                ).ToList());

            // ایجاد یک شیء جدید از کلاس AddUserRoleDto و ارسال آن به نما
            return View(new AddUserRoleDto
            {
                // انتساب شناسه کاربر به Id در AddUserRoleDto
                Id = Id,
                // انتساب لیست نقش‌ها به Roles در AddUserRoleDto
                Roles = roles,
                // انتساب ایمیل کاربر به Email در AddUserRoleDto
                Email = user.Email,
                // ترکیب نام و نام خانوادگی کاربر و انتساب به FullName در AddUserRoleDto
                FullName = $"{user.FirstName}  {user.LastName}"
            });
        }

        [HttpPost]
        // متد اکشن AddUserRole برای پردازش درخواست افزودن نقش به کاربر
        public IActionResult AddUserRole(AddUserRoleDto newRole)
        {
            // پیدا کردن کاربر با شناسه مشخص شده در مدل و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(newRole.Id).Result;

            // افزودن نقش به کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var result = _userManager.AddToRoleAsync(user, newRole.Role).Result;

            // هدایت کاربر به اکشن UserRoles از کنترلر Users در ناحیه admin با ارسال Id کاربر
            return RedirectToAction("UserRoles", "Users", new { Id = user.Id, area = "admin" });
        }

        // متد اکشن UserRoles برای نمایش لیست نقش‌های یک کاربر
        public IActionResult UserRoles(string Id)
        {
            // پیدا کردن کاربر با شناسه مشخص شده و منتظر نتیجه با استفاده از Result
            var user = _userManager.FindByIdAsync(Id).Result;

            // دریافت لیست نقش‌های کاربر با استفاده از _userManager و منتظر نتیجه با استفاده از Result
            var roles = _userManager.GetRolesAsync(user).Result;

            // ذخیره اطلاعات کاربر در ViewBag برای نمایش در نما
            ViewBag.UserInfo = $"Name : {user.FirstName} {user.LastName} Email:{user.Email}";

            // ارسال لیست نقش‌ها به نمای مربوطه
            return View(roles);
        }
    }
}

