using Microsoft.AspNetCore.Mvc;
using PropertyBL.Interfaces;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.Controllers
{
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IUserRepository _userRepository;
        private readonly IPropertyTypeRepository _typeRepository;
        public UserController(IUserRepository userRepository ,IPropertyTypeRepository propertyTypeRepository)
        {
            _userRepository = userRepository;
            _typeRepository = propertyTypeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test(int id=1) {
            var test = new Property
            {
                Id = id,
                Name = "vila",

            };
            return View();
        }
    }
}
