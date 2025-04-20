using Microsoft.AspNetCore.Mvc;
using PropertyBL.Interfaces;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.ViewModels;

namespace PropertyRentalMarketplace.Controllers
{
    
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IPropertyRepository _propertyRepository;
        public UserController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<IActionResult> Index()
        {
            var images = new List<Image>
            {
                new Image { Id = 1, Path = "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
                new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 }
            };
            var property = new PropertyViewModel()
                {
                    Id = 1,
                    Name = "Name",
                    Address = "LLL",
                    BedRooms = 3,
                    BathRooms = 2,
                    BetsAllowd = 0,
                    GarageSlots = 1,
                    IsListed = true,
                    IsFeatured = true,
                    ListedAt = DateTime.Now,
                    UnListDate = (DateTime.Now).AddDays(12),
                    ListingType = ListingType.Rent,
                    Images=images

                };
               
            
            var model = new List<PropertyViewModel> { property };
            return View(model);
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
