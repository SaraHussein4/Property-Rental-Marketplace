using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBL.Interfaces;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.ViewModels;
using System.Security.Claims;

namespace PropertyRentalMarketplace.Controllers
{
    
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        public UserController(IPropertyRepository propertyRepository, IFavouriteRepository favouriteRepository)
        {
            _propertyRepository = propertyRepository;
            _favouriteRepository = favouriteRepository;
        }
        public async Task<IActionResult> Index()
        {
            var images = new List<Image>
            {
                new Image { Id = 1, Path = "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
                new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
                new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
                new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
             
            };
            var images2 = new List<Image>
            {
               new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 2},
                new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 2}
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
                    IsFeatured = false,
                    ListedAt = DateTime.Now,
                    UnListDate = (DateTime.Now).AddDays(12),
                    ListingType = ListingType.Rent,
                    Images=images

                };
            var property1 = new PropertyViewModel()
            {
                Id = 2,
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
                Images = images2
            };


            var allProperities = new List<PropertyViewModel> { property ,property1};
            var featuedModel = allProperities.Where(model=>model.IsFeatured==true).ToList();
            var model = new PropertyPageViewModel
            {
                AllProperities = allProperities,
                FeaturedProperities = featuedModel
            };
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
