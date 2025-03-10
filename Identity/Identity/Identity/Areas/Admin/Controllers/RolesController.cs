using Identity.Areas.Admin.Data.Dto;
using Identity.Areas.Admin.Data.Dto.Roles;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Identity.Areas.Admin.Controllers
{
    // این خط مشخص می‌کند که این کنترلر در ناحیه Admin قرار دارد
    [Area("Admin")]
    public class RolesController : Controller
    {
        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت نقش‌ها
        private readonly RoleManager<Role> _roleManager;

        // تعریف یک فیلد خصوصی فقط-خواندنی برای مدیریت کاربران
        private readonly UserManager<User> _userManager;

        // سازنده کلاس که دو پارامتر برای مدیریت نقش‌ها و کاربران دریافت می‌کند (تزریق وابستگی)
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // متد اکشن Index که لیست نقش‌ها را نمایش می‌دهد
        public IActionResult Index()
        {
                // دریافت لیست تمام نقش‌ها از _roleManager
            var roles = _roleManager.Roles
                // تبدیل هر نقش به یک شیء RoleListDto با استفاده از LINQ
                .Select(p =>
                new RoleListDto
                {
                    // انتساب شناسه نقش به Id در RoleListDto
                    Id = p.Id,
                    // انتساب نام نقش به Name در RoleListDto
                    Name = p.Name,
                    // انتساب توضیحات نقش به Description در RoleListDto
                    Description = p.Description,
                })
            // تبدیل نتیجه به یک لیست
                .ToList();

            // ارسال لیست نقش‌ها به نمای مربوطه
            return View(roles);
        }

        [HttpGet]
        // متد اکشن Create برای نمایش فرم ایجاد نقش جدید
        public IActionResult Create()
        {
            // بازگرداندن نمای پیش‌فرض بدون مدل
            return View();
        }

        [HttpPost]
        // متد اکشن Create برای پردازش فرم ارسالی ایجاد نقش جدید
        public IActionResult Create(AddNewRoleDto newRole)
        {
            // ایجاد یک شیء جدید از کلاس Role
            Role role = new Role()
            {
                // انتساب نام از مدل دریافتی به شیء نقش جدید
                Name = newRole.Name,
                // انتساب توضیحات از مدل دریافتی به شیء نقش جدید
                Description = newRole.Description,
            };

            // ایجاد نقش جدید با استفاده از _roleManager و منتظر نتیجه با استفاده از Result
            var result = _roleManager.CreateAsync(role).Result;
            // بررسی موفقیت‌آمیز بودن عملیات ایجاد نقش
            if (result.Succeeded)
            {
                // هدایت کاربر به اکشن Index از کنترلر Roles در ناحیه Admin
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            };

            // ذخیره لیست خطاها در ViewBag برای نمایش در نما
            ViewBag.Errors = result.Errors.ToList();

            // بازگرداندن نما با همان مدل دریافتی
            return View(newRole);
        }

        // متد اکشن UserInRole برای نمایش لیست کاربران دارای یک نقش خاص
        public IActionResult UserInRole(string Name)
        {
            // دریافت لیست کاربران دارای نقش مشخص شده و منتظر نتیجه با استفاده از Result
            var userInRole = _userManager.GetUsersInRoleAsync(Name).Result;

            // تبدیل هر کاربر به یک شیء UserListDto با استفاده از LINQ و ارسال به نما
            return View(userInRole.Select(p => new UserListDto
            {
                // انتساب اطلاعات کاربر به در UserListDto
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName = p.UserName,
                PhoneNumber = p.PhoneNumber,
                Id = p.Id,
            }));
        }
    }
}
