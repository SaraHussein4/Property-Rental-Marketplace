using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PropertyRentalMarketplace.Controllers
{

    //[Authorize(Roles = AppRoles.User)]

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
        private readonly INotificationRepository _notificationRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<User> _userManager;


        public UserController(IPropertyRepository propertyRepository
             ,IImageRepository imageRepository 
            ,IUserRepository userRepository ,IAmenityRepository amenityRepository
            ,IPropertyAmenityRepository propertyAmenityRepository
            ,IServiceRepository serviceRepository ,ICountryRepository countryRepository
            ,IFavouriteRepository favouriteRepository1 , IPropertyTypeRepository propertyTypeRepository
            , INotificationRepository notificationRepository, IRatingRepository ratingRepository, 
            IBookingRepository bookingRepository, UserManager<User> userManager
            )    
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
            _notificationRepository = notificationRepository;
            _ratingRepository = ratingRepository;
            _bookingRepository = bookingRepository;
            _userManager = userManager;

        }
        #region index
        public async Task<IActionResult> Index()
        {
            var allProperities = (await _propertyRepository.GetAll()).OrderByDescending(p=>p.ListedAt).Take(4).ToList();
            var featuredModel = await _propertyRepository.GetAllFeatured();
            var topRating = (await _propertyRepository.GetTopRating1()).Take(4).ToList();
            var model = new PropertyPageViewModel
            {
                AllProperities = allProperities,
                FeaturedProperities = featuredModel,
                RatingProperty = topRating,

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allrev = await _ratingRepository.GetallRatingbyid(id);
            //var isFav = await _favouriteRepository1.isfav(userId, id);

            var model = new PropertyViewModel
            {
                Property =data,
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
                Latitude = data.Location.Latitude,
                Longitude = data.Location.Longitude,
                ImagesHost = imghost,
                Images = images,
                PhoneNumber = data.Host.PhoneNumber,
                Email=data.Host.Email,
                amenities = AllAmenities.ToList(),
                safeties = safeties.ToList(),
                services=allServices.ToList(),
                CurrentUserId = userId,
                Host = data.Host,
                ratings=allrev,
                //isFav = isFav 
            };
            
            
            return View("Details",model);
        }
        #endregion

        #region add to favourite
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavourite(string userId, int propertyId)
        {
            if (string.IsNullOrEmpty(userId) || propertyId <= 0)
            {
                return Json(new { success = false, message = "success" });
            }
            try
            {

                await _favouriteRepository1.AddToFavourite(userId, propertyId);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public async Task<IActionResult> ShowFav( PropertyViewModel propertyViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allfav = await _favouriteRepository1.getallfav(userId);
            return View("ShowFav",allfav);
        }
        #endregion
        #region remove from  favourite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavourite(string userId, int propertyId)
        {
            try
            {
                await _favouriteRepository1.RemoveToFavourite(userId, propertyId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
          
        }

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
        [HttpGet]
        public async Task<IActionResult> Notification()
        {
            List<Notification> notifications = await _notificationRepository.GetNotificationsForUser("00083eec-d2e1-44f1-b6d1-e9d87946a505");
            return View(notifications);
        }



        [HttpGet]
        public async Task<IActionResult> Rate(int bookingId)
        {
            UserRateViewModel model = new UserRateViewModel
            {
                BookingId = bookingId
            };
            return View(model);
        }


        public async Task<IActionResult> Rate(UserRateViewModel model)
        {
            try
            {
                await _ratingRepository.BeginTransactionAsync();
                Rating rating = new Rating
                {
                    OverallRating = model.OverallRating,
                    AmenitiesRating = model.AmenitiesRating,
                    CommunicationRating = model.CommunicationRating,
                    ValueForMoneyRating = model.ValueForMoneyRating,
                    HygieneRating = model.HygieneRating,
                    LocationRating = model.LocationRating,
                    Review = model.Review,
                    CreatedAt = DateTime.Now,
                    BookingId = model.BookingId,
                    UserId = "00083eec-d2e1-44f1-b6d1-e9d87946a505"
                };
                await _ratingRepository.Add(rating);
                await _ratingRepository.Save();


                var notification = await _notificationRepository.GetNotificationForUserAndBooking("00083eec-d2e1-44f1-b6d1-e9d87946a505", model.BookingId);
                notification.IsReaded = true;
                await _notificationRepository.Save();

                Booking booking = await _bookingRepository.GetById(rating.BookingId);
                //Property property = await _propertyRepository.GetById(booking.PropertyId);
                //property.StarRating = await _propertyRepository.GetStarRating(property.Id);
                await _propertyRepository.Save();

                await _ratingRepository.CommitAsync();
            }
                  
            catch (Exception ex) {
                await _ratingRepository.RollbackAsync();
            }

            return RedirectToAction("notification");

        }

        public string uploadImage(IFormFile ImgUrl)
        {
            string fileName = null;

            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImgUrl.CopyTo(stream);
                }
            }
            return fileName;
        }

        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var viewModel = new UserProfileEditViewModel
            {
                Name = user.Name,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                Image = user.Image,
                Email = user.Email,
                Id = user.Id,

            };

            return View(viewModel);
        }
        public async Task<IActionResult> EditProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var viewModel = new UserProfileEditViewModel
            {
                Name = user.Name,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                Image = user.Image,
                Email = user.Email,
                Id = user.Id,

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(UserProfileEditViewModel userFromRequest)
        {
            //if (!ModelState.IsValid)
            //    return View("EditProfile", userFromRequest);

            var user = await _userManager.FindByIdAsync(userFromRequest.Id);
            if (user == null)
                return NotFound();

            // Update editable fields
            user.Name = userFromRequest.Name;
            user.PhoneNumber = userFromRequest.PhoneNumber;
            user.Gender = userFromRequest.Gender;

            try
            {
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", new { id = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", message);
            }

            return View("EditProfile", userFromRequest);

        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(string id, IFormFile ImgUrl)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && ImgUrl != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(user.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", user.Image);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // Upload new image
                string fileName = uploadImage(ImgUrl);
                user.Image = fileName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, imageFileName = fileName });
                }
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var viewModel = new ChangePasswordViewModel
            {
                Id = user.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction("EditProfile", new { id = model.Id });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

    }
}
