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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PropertyRentalMarketplace.Controllers
{

 
    [Authorize(Roles = AppRoles.User)]
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly IPropertyAmenityRepository _propertyAmenityRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IFavouriteRepository _favouriteRepository1;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        public UserController(IPropertyRepository propertyRepository
             ,IImageRepository imageRepository 
            ,IUserRepository userRepository ,IAmenityRepository amenityRepository
            ,IPropertyAmenityRepository propertyAmenityRepository
            ,IServiceRepository serviceRepository ,ICountryRepository countryRepository
            ,IFavouriteRepository favouriteRepository1 , IPropertyTypeRepository propertyTypeRepository)    
        {
            _propertyRepository = propertyRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _amenityRepository = amenityRepository;
            _propertyAmenityRepository = propertyAmenityRepository;
            _serviceRepository = serviceRepository;
            _countryRepository = countryRepository;
            _favouriteRepository1 = favouriteRepository1;
            _propertyTypeRepository = propertyTypeRepository;
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

        public async Task<IActionResult> FindProperty()
        {
            var types = (await _propertyTypeRepository.GetAll()).ToList();
            var defaultId = types.FirstOrDefault()?.Id ?? 0;
            var props = await _propertyRepository.GetPropertyTypeById(defaultId);

            var model = new FindPropertyViewModel
            {
                PropertyTypes = types,
                InitialProperties = props
            };

            return View(model);
        }

        public async Task<JsonResult> GetPropertiesByType(int typeId)
        {
            var props = await _propertyRepository.GetPropertyTypeById(typeId);
            return Json(props.Select(p => new {
                id = p.Id,
                name = p.Name,
                address = p.Address,
                bedRooms = p.BedRooms,
                bathRooms = p.BathRooms, // Fixed typo from your original code
                petsAllowed = p.BetsAllowd,
                garageSlots = p.GarageSlots,
                images = p.Images.Select(i => new {
                    path = i.Path.StartsWith("http") ? i.Path : $"/images/{i.Path}"
                }).ToList()
            }));
        }



        [HttpPost]
        public async Task<IActionResult> GetFilteredProperties([FromBody] PropertyFilters filters)
        {
            try
            {
                // Implement your filtering logic here based on the filters object
                var filteredProperties = await _propertyRepository.GetFilteredProperties(
                    filters.TypeId,
                    filters.PriceRanges,
                    filters.Countries,
                    filters.Bedrooms);

                return Ok(filteredProperties.Select(p => new {
                    id = p.Id,
                    name = p.Name,
                    address = p.Address,
                    bedRooms = p.BedRooms,
                    bathRooms = p.BathRooms,
                    garageSlots = p.GarageSlots,
                    petsAllowed = p.BetsAllowd,
                    images = p.Images.Select(i => new {
                        path = i.Path.StartsWith("http") ? i.Path : $"/images/{i.Path}"
                    })
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
