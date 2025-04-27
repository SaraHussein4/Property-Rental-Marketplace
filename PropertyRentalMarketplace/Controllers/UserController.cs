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
        private readonly IPropertyAmenityRepository _propertyAmenityRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IFavouriteRepository _favouriteRepository1;

        public UserController(IPropertyRepository propertyRepository
            , IFavouriteRepository favouriteRepository ,IImageRepository imageRepository 
            ,IUserRepository userRepository ,IAmenityRepository amenityRepository
            ,IPropertyAmenityRepository propertyAmenityRepository
            ,IServiceRepository serviceRepository ,ICountryRepository countryRepository
            ,IFavouriteRepository favouriteRepository1)
        {
            _propertyRepository = propertyRepository;
            _favouriteRepository = favouriteRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _amenityRepository = amenityRepository;
            _propertyAmenityRepository = propertyAmenityRepository;
            _serviceRepository = serviceRepository;
            _countryRepository = countryRepository;
            _favouriteRepository1 = favouriteRepository1;
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
            var imghost = await _propertyRepository.getimagehost(id);
            var safeties = await _amenityRepository.GetSafetyById(id);
            var AllAmenities = await _amenityRepository.GetAllAmenitiesById(id);
            var allServices =await _serviceRepository.GetAllServicesById(id);
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
                BetsAllowd = data.BetsAllowd,
                GarageSlots = data.GarageSlots,
                FeesPerMonth = data.FeesPerMonth,
                ImagesHost = imghost,
                Images = images,
                 PhoneNumber = data.Host.PhoneNumber,
                 Email=data.Host.Email,
                amenities = AllAmenities.ToList(),
                safeties = safeties.ToList(),
                services=allServices.ToList(),
            };
            
            
            return View("Details",model);
        }
        #endregion
        #region add to favourite
        [HttpPost]
        public async Task<IActionResult> AddToFavourite(string userId, int propertyId)
        {
            try
            {
                // إضافة العقار إلى المفضلة
                await _favouriteRepository.AddToFavourite(userId, propertyId);

                // إرسال استجابة ناجحة
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ
                return Json(new { success = false, message = ex.Message });
            }
        }
        //public async Task<IActionResult> AddToFavourite(string userid,int propid)
        //{
        //    await _favouriteRepository.AddToFavourite(userid, propid);
        //    return View("AddToFavourite");
        //}
        #endregion
        #region remove from  favourite
        //public async Task<IActionResult> RemoveFromFavourite(string userid, int propid)
        //{
        //    await _favouriteRepository.AddToFavourite(userid, propid);
        //    return RedirectToAction("AddToFavourite");
        //}
        #endregion
    }
}
