using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

using PropertyDAL.Models;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.Helpers;
using PropertyRentalMarketplace.ViewModels;

namespace PropertyRentalMarketplace.Controllers
{
    public class AccountController : Controller
    {
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
                    UserName = model.Email.Split('@')[0],
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

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid) { 
              User userFromDb = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, loginViewModel.Password);
                    if (found)
                    {
                     var LoginResult =   await signInManager.PasswordSignInAsync(userFromDb, loginViewModel.Password,loginViewModel.RememberMe,false);
                        if (LoginResult.Succeeded)
                        {
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password Is Incorrect");
                    }
                }
                else
                ModelState.AddModelError(string.Empty,"Email is not Exists");
            }
            return View("Login",loginViewModel);
        }
        public new async Task<IActionResult> SignOut()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User= await userManager.FindByEmailAsync(model.Email);
                if (User is not null) 
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword","Account", new {email = User.Email ,Token =token },Request.Scheme);

                    var Email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink

                    };
                    EmailSettings.SendEmail(Email);
                    return RedirectToAction(nameof(CheckYourInbox));

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Exists");

                }
            }
          
                return View("ForgetPassword",model);
            
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"]=email;
            TempData["token"]=token;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await userManager.FindByEmailAsync(email);
                var Result = await userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
