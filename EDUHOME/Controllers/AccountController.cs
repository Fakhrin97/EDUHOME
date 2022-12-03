using EDUHOME.DAL.Entities;
using EDUHOME.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EDUHOME.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                RetunUrl = returnUrl,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.FindByNameAsync(model.UserName);

            if (existUser == null)
            {
                ModelState.AddModelError("", "username yalnishdir");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RemeberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "shifre yalnishdir");
                return View();
            }

            if (string.IsNullOrEmpty(model.RetunUrl))
                return RedirectToAction("index", "home");

            return Redirect(model.RetunUrl);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}
