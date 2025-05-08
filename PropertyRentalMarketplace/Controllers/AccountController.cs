using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

using PropertyDAL.Models;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.Helpers;
using PropertyRentalMarketplace.ViewModels;
using System.Security.Claims;

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
        public IActionResult ExternalRegister(string provider, string returnUrl = null, string role = null)
        {
            var allowedRoles = new[] { AppRoles.Host, AppRoles.User };
            role = allowedRoles.Contains(role) ? role : AppRoles.User;

            var redirectUrl = Url.Action("ExternalRegisterCallback", "Account", new { returnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            properties.Items["role"] = role;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                properties.Items["returnUrl"] = returnUrl;
            }

            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalRegisterCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Register");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Register");

            var allowedRoles = new[] { AppRoles.Host, AppRoles.User };
            var role = info.AuthenticationProperties.Items.TryGetValue("role", out var extractedRole)
                && allowedRoles.Contains(extractedRole)
                    ? extractedRole
                    : AppRoles.User;

            var returnUrl = info.AuthenticationProperties.Items.TryGetValue("returnUrl", out var extractedReturnUrl)
                ? extractedReturnUrl
                : null;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await signInManager.SignInAsync(existingUser, isPersistent: false);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = role == AppRoles.Host
                        ? Url.Action("Index", "Host")
                        : Url.Action("Index", "User");
                }

                return LocalRedirect(returnUrl);
            }

            var newUser = new User
            {
                UserName = email.Split('@')[0],
                Email = email,
                Name = name,
                IsAgree = true
            };

            var createResult = await userManager.CreateAsync(newUser);
            if (createResult.Succeeded)
            {
                await userManager.AddLoginAsync(newUser, info);

                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

                await userManager.AddToRoleAsync(newUser, role);
                await signInManager.SignInAsync(newUser, isPersistent: false);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    var roles = await userManager.GetRolesAsync(newUser);
                    if (roles.Contains(AppRoles.Admin))
                        returnUrl = Url.Action("Index", "Admin");
                    else if (roles.Contains(AppRoles.Host))
                        returnUrl = Url.Action("Index", "Host");
                    else
                        returnUrl = Url.Action("Index", "User");
                }

                return LocalRedirect(returnUrl);
            }

            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToAction("Index", "User");
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

                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
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


        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null, string returnUrl = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                
                TempData["ExternalProviderError"] = "User not registered. Please sign up first.";
                return RedirectToAction("Login");
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains(AppRoles.Admin))
                return RedirectToAction("Index", "Admin");
            else if (roles.Contains(AppRoles.Host))
                return RedirectToAction("Index", "Host");
            else
                return RedirectToAction("Index", "User");
        }

    }
}
