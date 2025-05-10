using Microsoft.AspNetCore.Authentication;
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

                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                var User = new User()
                {
                    UserName = model.Email.Split('@')[0],
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
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
            properties.Items["scheme"] = role == AppRoles.Host ? "HostScheme" : "UserScheme";

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

            var scheme = info.AuthenticationProperties.Items.TryGetValue("scheme", out var extractedScheme)
                        ? extractedScheme
                        : (role == AppRoles.Host ? "HostScheme" : "UserScheme");

            var returnUrl = info.AuthenticationProperties.Items.TryGetValue("returnUrl", out var extractedReturnUrl)
                           ? extractedReturnUrl
                           : null;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await HttpContext.SignOutAsync();

                var principal = await signInManager.CreateUserPrincipalAsync(existingUser);
                var identity = (ClaimsIdentity)principal.Identity;
                identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, scheme));

                await HttpContext.SignInAsync(scheme, principal);

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

             
                var principal = await signInManager.CreateUserPrincipalAsync(newUser);
                var identity = (ClaimsIdentity)principal.Identity;
                identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, scheme));

                await HttpContext.SignInAsync(scheme, principal);

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

            return RedirectToAction("Register");
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

      
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(model);
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);

               
                await HttpContext.SignOutAsync();

                if (roles.Contains("Host"))
                {
                    var hostPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                    var hostIdentity = (ClaimsIdentity)hostPrincipal.Identity;
                    hostIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "HostScheme"));

                    await HttpContext.SignInAsync("HostScheme", hostPrincipal, new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    });

                    return RedirectToAction("Index", "Host");
                }
                else
                {
                    var userPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                    var userIdentity = (ClaimsIdentity)userPrincipal.Identity;
                    userIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "UserScheme"));

                    await HttpContext.SignInAsync("UserScheme", userPrincipal, new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    });

                    return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("UserScheme");
            await HttpContext.SignOutAsync("HostScheme");
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
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var roles = await userManager.GetRolesAsync(user);

                await HttpContext.SignOutAsync();

                if (roles.Contains("Host"))
                {
                    var hostPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                    await HttpContext.SignInAsync("HostScheme", hostPrincipal);
                    return RedirectToAction("Index", "Host");
                }
                else
                {
                    var userPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                    await HttpContext.SignInAsync("UserScheme", userPrincipal);
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                return RedirectToAction("Login");

            }
        }

    }
}
