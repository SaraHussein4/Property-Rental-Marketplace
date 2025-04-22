using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropertyDAL.Models;
using PropertyRentalMarketplace.ViewModels;

namespace PropertyRentalMarketplace.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;

        public AccountController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var User = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    Gender = model.Gender
                };
           var Result = await userManager.CreateAsync(User , model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    
                }
            }
            return View(model);
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PhoneLogin()
        {
            return View();
        }
        public IActionResult EmailLogin()
        {
            return View();
        }
        public IActionResult EnterPassword()
        {
            return View();
        }
        public IActionResult SetPassword()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
