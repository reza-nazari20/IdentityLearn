using Identity.Areas.Admin.Data.Dto;
using Identity.Areas.Admin.Data.Dto.Roles;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    // این خط مشخص می‌کند که این کنترلر در ناحیه Admin قرار دارد
    public class RolesController : Controller
    // تعریف کلاس RolesController که از کلاس Controller ارث‌بری می‌کند
    {
        private readonly RoleManager<Role> _roleManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت نقش‌ها
        private readonly UserManager<User> _userManager;
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت کاربران

        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        // سازنده کلاس که دو پارامتر برای مدیریت نقش‌ها و کاربران دریافت می‌کند (تزریق وابستگی)
        {
            _roleManager = roleManager;
            // مقداردهی فیلد _roleManager با مقدار دریافتی از پارامتر
            _userManager = userManager;
            // مقداردهی فیلد _userManager با مقدار دریافتی از پارامتر
        }

        public IActionResult Index()
        // متد اکشن Index که لیست نقش‌ها را نمایش می‌دهد
        {
            var roles = _roleManager.Roles
                // دریافت لیست تمام نقش‌ها از _roleManager
                .Select(p =>
                // تبدیل هر نقش به یک شیء RoleListDto با استفاده از LINQ
                new RoleListDto
                {
                    Id = p.Id,
                    // انتساب شناسه نقش به Id در RoleListDto
                    Name = p.Name,
                    // انتساب نام نقش به Name در RoleListDto
                    Description = p.Description,
                    // انتساب توضیحات نقش به Description در RoleListDto
                })
                .ToList();
            // تبدیل نتیجه به یک لیست
            return View(roles);
            // ارسال لیست نقش‌ها به نمای مربوطه
        }

        [HttpGet]
        // مشخص می‌کند که این متد فقط به درخواست‌های GET پاسخ می‌دهد
        public IActionResult Create()
        // متد اکشن Create برای نمایش فرم ایجاد نقش جدید
        {
            return View();
            // بازگرداندن نمای پیش‌فرض بدون مدل
        }

        [HttpPost]
        // مشخص می‌کند که این متد فقط به درخواست‌های POST پاسخ می‌دهد
        public IActionResult Create(AddNewRoleDto newRole)
        // متد اکشن Create برای پردازش فرم ارسالی ایجاد نقش جدید
        {
            Role role = new Role()
            // ایجاد یک شیء جدید از کلاس Role
            {
                Name = newRole.Name,
                // انتساب نام از مدل دریافتی به شیء نقش جدید
                Description = newRole.Description,
                // انتساب توضیحات از مدل دریافتی به شیء نقش جدید
            };
            var result = _roleManager.CreateAsync(role).Result;
            // ایجاد نقش جدید با استفاده از _roleManager و منتظر نتیجه با استفاده از Result
            if (result.Succeeded)
            // بررسی موفقیت‌آمیز بودن عملیات ایجاد نقش
            {
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
                // هدایت کاربر به اکشن Index از کنترلر Roles در ناحیه Admin
            };
            ViewBag.Errors = result.Errors.ToList();
            // ذخیره لیست خطاها در ViewBag برای نمایش در نما
            return View(newRole);
            // بازگرداندن نما با همان مدل دریافتی
        }

        public IActionResult UserInRole(string Name)
        // متد اکشن UserInRole برای نمایش لیست کاربران دارای یک نقش خاص
        {
            var userInRole = _userManager.GetUsersInRoleAsync(Name).Result;
            // دریافت لیست کاربران دارای نقش مشخص شده و منتظر نتیجه با استفاده از Result
            return View(userInRole.Select(p => new UserListDto
            // تبدیل هر کاربر به یک شیء UserListDto با استفاده از LINQ و ارسال به نما
            {
                FirstName = p.FirstName,
                // انتساب نام کاربر به FirstName در UserListDto
                LastName = p.LastName,
                // انتساب نام خانوادگی کاربر به LastName در UserListDto
                UserName = p.UserName,
                // انتساب نام کاربری به UserName در UserListDto
                PhoneNumber = p.PhoneNumber,
                // انتساب شماره تلفن کاربر به PhoneNumber در UserListDto
                Id = p.Id,
                // انتساب شناسه کاربر به Id در UserListDto
            }));
        }
    }
}
