using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    public class UserController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        public UserController(IPropertyRepository propertyRepository, IFavouriteRepository favouriteRepository)
        {
            _propertyRepository = propertyRepository;
            _favouriteRepository = favouriteRepository;
        }
        public async Task<IActionResult> Index()
        {
            var allProperities = (await _propertyRepository.GetAll()).OrderByDescending(p=>p.ListedAt).Take(4).ToList();
            var featuedModel = await _propertyRepository.GetAllFeatured();
            var model = new PropertyPageViewModel
            {
                AllProperities = allProperities,
                FeaturedProperities = featuedModel
            };
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var images = new List<Image>
            {
                new Image { Id = 1, Path = "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
                new Image { Id = 2, Path= "https://images.pexels.com/photos/265004/pexels-photo-265004.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 3, Path= "https://images.pexels.com/photos/271624/pexels-photo-271624.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 4, Path= "https://images.pexels.com/photos/1918291/pexels-photo-1918291.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 5, Path= "https://images.pexels.com/photos/1643384/pexels-photo-1643384.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 6, Path= "https://images.pexels.com/photos/259962/pexels-photo-259962.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 7, Path= "https://images.pexels.com/photos/1457847/pexels-photo-1457847.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 8, Path= "https://images.pexels.com/photos/1571453/pexels-photo-1571453.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 9, Path= "https://images.pexels.com/photos/265004/pexels-photo-265004.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
                new Image { Id = 10, Path= "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBU3UXoeGSiQtW0no3PdFq_g_rKEWKS9flyw&s", PropertyId = 1 }

            };
           
            var property = new PropertyViewModel()
            {
                Id = 1,
                Name = "Test",
                Address = "LLL",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.  \r\nUt enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.  \r\nDuis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.  \r\nExcepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\r\n",
                BedRooms = 3,
                BathRooms = 2,
                BetsAllowd = 0,
                GarageSlots = 1,
                IsListed = true,
                IsFeatured = true,
                ListedAt = DateTime.Now,
                UnListDate = (DateTime.Now).AddDays(12),
                ListingType = ListingType.Rent,
                Images = images

            };


            var model = new List<PropertyViewModel> { property };
           
            if(model == null)
            {
                return View("Error");
            }
                return View("Details" , property);
        }
    }
}
