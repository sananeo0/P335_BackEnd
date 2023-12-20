using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P335_BackEnd.Areas.Admin.Models;
using P335_BackEnd.Entities;

namespace P335_BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = await _userManager.FindByNameAsync(model.Email);

            if(existingUser is null)
            {
                model.ErrorMessage = "Username or password is incorrect!";
                return View(model);
            }

            var result = await _signInManager
                .PasswordSignInAsync(existingUser, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                model.ErrorMessage = "Username or password is incorrect!";
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var newUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            };

            var result =  await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                model.ErrorMessage = string.Join(" ", result.Errors.Select(x => x.Description));
                return View(model);
            }

            Console.WriteLine(result.ToString());
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}