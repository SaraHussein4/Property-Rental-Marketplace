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
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<User> userManager ,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register(string role)
        {
            var allowedRoles = new[] { AppRoles.Host, AppRoles.User };
            ViewBag.Role = allowedRoles.Contains(role) ? role : AppRoles.User;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string role)
        {
            Console.WriteLine($"DEBUG: Received role parameter: {role}");
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
                  
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await userManager.AddToRoleAsync(User, role);

                    await signInManager.SignInAsync(User, isPersistent: false);
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        model.RememberMe,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                      
                        var roles = await userManager.GetRolesAsync(user);

           
                        if (roles.Contains(AppRoles.Admin))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (roles.Contains(AppRoles.Host))
                        {
                            return RedirectToAction("Index", "Host");
                        }
                        else
                        {
                            return RedirectToAction("Index", "User");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
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
