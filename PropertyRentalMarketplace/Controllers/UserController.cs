using Microsoft.AspNetCore.Mvc;

namespace PropertyRentalMarketplace.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
