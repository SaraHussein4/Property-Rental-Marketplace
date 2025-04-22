using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

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

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager ,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
<<<<<<< HEAD
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
=======
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
            return View();
>>>>>>> b07bf7f07108d5c3fba15340b5bd54d10fe729ba
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
        public IActionResult Register()

        {
            return View();
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid) { 
              User userFromDb = await userManager.FindByNameAsync(loginViewModel.UserName);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, loginViewModel.Password);

                    if (found)
                    {
                        //create cookie
                        await signInManager.SignInAsync(userFromDb, loginViewModel.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("","Error login");
            }
            return View("Login",loginViewModel);
        }
        public IActionResult Logout()
        { 
            return View();
        }
    }
}
