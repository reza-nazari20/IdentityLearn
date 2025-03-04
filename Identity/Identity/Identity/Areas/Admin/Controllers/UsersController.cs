using Identity.Areas.Admin.Data.Dto;
using Identity.Models.Dto;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                .Select(p => new UserListDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    EmailConfirmed = p.EmailConfirmed,
                    AccessFailedCount = p.AccessFailedCount
                }).ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }

            User newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "users", new { area = "admin" });
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(register);
        }

        public IActionResult Edit(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            UserEditDto userEdit = new UserEditDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };

            return View(userEdit);
        }

        [HttpPost]
        public IActionResult Edit(UserEditDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            user.FirstName = userEdit.FirstName;
            user.LastName = userEdit.LastName;
            user.Email = userEdit.Email;
            user.UserName = userEdit.UserName;
            user.PhoneNumber = userEdit.PhoneNumber;

            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "users", new { area = "admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(userEdit);
        }

        public IActionResult Delete(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            UserDeleteDto userDelete = new UserDeleteDto()
            {
                Id = user.Id,
                FullName = $"{user.FirstName}{user.LastName}",
                Email = user.Email,
                UserName = user.UserName,
            };
            return View(userDelete);
        }

        [HttpPost]
        public IActionResult Delete(UserDeleteDto userDelete)
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;

            var result = _userManager.DeleteAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "users", new { area = "admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(userDelete);
        }
    }
}
