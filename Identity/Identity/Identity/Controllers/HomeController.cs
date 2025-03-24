using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // این اکشن فقط برای کاربرانی قابل دسترسی است که کلیم "Buyer" را داشته باشند
        // این اکشن برای کسانی قابل دسترسی است که پالیسی Buyبرای آنها تنظیم شده باشد و این پالیسی برای کسانی است که کلیم "Buyer" را داشته باشند
        [Authorize(Policy = "Buy")]
        public IActionResult JustBuyer()
        {
            return View();
        }

        // این اکشن فقط برای کاربرانی قابل دسترسی است که کلیم "Blood" با مقدار "A+" یا "O+" داشته باشند
        // این اکشن فقط برای کسانی در دسترس است که پالیسی BloodType را داشته باشند و این پالیسی برای کسانی است که کلیم "Blood" با مقدار "A+" یا "O+" داشته باشند
        [Authorize(Policy = "BloodType")]
        public IActionResult BloodHuman()
        {
            return View();
        }

        // این اکشن فقط برای کاربرانی قابل دسترسی است که کلیم "Cradit" داشته باشند و مقدار آن حداقل 10000 باشد
        [Authorize(policy: "Cradit")]
        public IActionResult Credit()
        {
            return View();
        }

        // این اکشن برای همه کاربران قابل دسترسی است (حتی کاربران وارد نشده به سیستم)
        // معمولاً برای نمایش صفحه خطای "دسترسی غیرمجاز" استفاده می‌شود
        // وقتی کاربر به صفحه‌ای دسترسی ندارد، به این صفحه هدایت می‌شود
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
