using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using PropertyBL.Interfaces;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.ViewModels;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyRentalMarketplace.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAmenityRepository _amenityRepository;

        public UserController(IPropertyRepository propertyRepository
            , IFavouriteRepository favouriteRepository ,IImageRepository imageRepository 
            ,IUserRepository userRepository ,IAmenityRepository amenityRepository)
        {
            _propertyRepository = propertyRepository;
            _favouriteRepository = favouriteRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _amenityRepository = amenityRepository;
        }
        #region index
        public async Task<IActionResult> Index()
        {
            var allProperities =(await _propertyRepository.GetAll()).OrderByDescending(p=>p.ListedAt).ToList();
            var featuredModel = await _propertyRepository.GetAllFeatured();
            var model = new PropertyPageViewModel
            {
                AllProperities = allProperities,
                FeaturedProperities = featuredModel
            };
            return View(model);
        }
        #endregion
        #region details
        public async Task<IActionResult> Details(int id, PropertyViewModel propertyViewModel ,UserProfileEditViewModel userProfileEditViewModel)
        {
            var data = await _propertyRepository.GetById(id);
            if (data == null)
            {
                return View("Error");
            }
            var images= await _imageRepository.GetImageById(id);
            var imghost = await _userRepository.getimagehost(id);
            //var Amenitie = await _amenityRepository.GetAllAmenitiesById(id);
            //var safeties = await _amenityRepository.GetSafeties();
            var model = new PropertyViewModel
            {
                Property = data,
                Address = data.Address,
                Description=data.Description,
                BedRooms = data.BedRooms,
                BathRooms = data.BedRooms,
                Name = data.Name,
                Area = data.Area,
                IsListed = data.IsListed,
                IsFavourite =data.IsFeatured,
                ListedAt = data.ListedAt,
                UnListDate = data.UnListDate,
                ListingType = data.ListingType,
                //Amenities = data.Amenities,
                BetsAllowd = data.BetsAllowd,
                GarageSlots = data.GarageSlots,
                FeesPerMonth = data.FeesPerMonth,
                ImagesHost = imghost,
                Images = images,
                 PhoneNumber = data.Host.PhoneNumber,
                 Email=data.Host.Email,
                //amenities = Amenitie,
                //safeties = safeties
            };
            
            
            return View("Details",model);
        }
        #endregion
    }
}
