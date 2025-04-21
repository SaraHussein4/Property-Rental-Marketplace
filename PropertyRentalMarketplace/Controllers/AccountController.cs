using Microsoft.AspNetCore.Mvc;

namespace PropertyRentalMarketplace.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
