using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    //تعریف اجازه ورود به کسانی که نقش آنها Admin یا Operator است در کل این کنترلر
    [Authorize(Roles ="Admin,Operator")]
    public class AuthorizeController : Controller
    {
        //پشتیبانی از همان تعریف اجازه ورود که برای کل کنترلر تعریف شده است
        public IActionResult Index()
        {
            return View();
        }

        // تعریف اجازه ورود به کسانی که نقش آنها Admin است
        [Authorize(Roles ="Admin")]
        public IActionResult Edit()
        {
            return View();
        }

        //اجازه ورود به همه بدون نیاز به ثبت نام و یا داشتن هیچ نقشی 
        [AllowAnonymous]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
