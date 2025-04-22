using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyBL.Interfaces;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.ViewModels;

namespace PropertyRentalMarketplace.Controllers
{
    public class HostController : Controller
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPropertyAmenityRepository _propertyAmenityRepository;
        private readonly IUserRepository _userRepository;

        public HostController(IPropertyTypeRepository propertyTypeRepository,
            IAmenityRepository amenityRepository, ICountryRepository countryRepository, IImageRepository imageRepository,
            ILocationRepository locationRepository, IPropertyRepository propertyRepository, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IServiceRepository serviceRepository, IPropertyAmenityRepository propertyAmenityRepository
            ,IUserRepository userRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _amenityRepository = amenityRepository;
            _countryRepository = countryRepository;
            _imageRepository = imageRepository;
            _locationRepository = locationRepository;
            _propertyRepository = propertyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceRepository = serviceRepository;
            _propertyAmenityRepository = propertyAmenityRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int propertyId)
        {
            Property property = await _propertyRepository.GetById(propertyId);
            HostEditPropertyViewModel model = new HostEditPropertyViewModel() { 
                Id = propertyId,
                Address = property.Address,
                Description = property.Description,
                CountryCode = property.Location.Country,
                StateCode = property.Location.State,
                Latitude = property.Location.Latitude,
                Longitude = property.Location.Longitude,
                Bedrooms = property.BedRooms,
                Bathrooms = property.BathRooms,
                GarageSlots = property.GarageSlots,
                BetsAllowed = property.BetsAllowd,
                Name = property.Name,
                FeesPerMonth = property.FeesPerMonth,
                ListingType = (int)property.ListingType,
                Area = property.Area,
                PropertyTypeId = property.PropertyTypeId,
                Safeties = new List<int>(),
                Amenities = new List<int>()
            };
            await PopulateHostEditPropertyViewModelAsync(model);

            foreach (PropertyAmenity amenity in property.Amenities)
            {
                if(amenity.Amenity.AmenityCategory.Name == "Amenity")
                {
                    model.Amenities.Add(amenity.AmenityId);
                }
                else 
                {
                    model.Safeties.Add(amenity.AmenityId);
                }
            }
            int i = 0;
            foreach(Service service in property.Services)
            {
                model.Services[i].Name = service.Name;
                model.Services[i].Distance = service.Distance;
                model.Services[i].StarRating = service.StarRating;
                i++;
            }
            model.ImagesUrls = new List<string>();
            foreach(Image img in property.Images)
            {
                if (img.IsPrimary)
                {
                    model.PrimaryImageUrl = img.Path;
                }
                else
                {
                    model.ImagesUrls.Add(img.Path);
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(HostEditPropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateHostEditPropertyViewModelAsync(model);
                return View(model);
            }
            try
            {
                Property property = await _propertyRepository.GetById(model.Id);
                
                if(property.Host.Id != "23d1c943-494f-489b-acaf-5144c2fe2387")
                {
                    return Json("You Can't Edit Property of Another Host");
                }

                await _propertyRepository.BeginTransactionAsync();

                property.Id = model.Id;
                property.Name = model.Name;
                property.Description = model.Description;
                property.Address = model.Address;
                property.BedRooms = model.Bedrooms;
                property.BathRooms = model.Bathrooms;
                property.GarageSlots = model.GarageSlots;
                property.BetsAllowd = model.BetsAllowed;
                property.Area = model.Area;
                property.FeesPerMonth = model.FeesPerMonth;
                property.ListingType = (ListingType)model.ListingType;
                property.PropertyTypeId = model.PropertyTypeId;

                property.Location.Latitude = model.Latitude;
                property.Location.Longitude = model.Longitude;
                property.Location.Country = model.CountryCode;
                property.Location.City = "Dummy";
                property.Location.State = model.StateCode;
                await _locationRepository.Save();

                for (int i = 0; i < model.Services.Count; ++i)
                {
                    property.Services[i].Name = model.Services[i].Name;
                    property.Services[i].Distance = model.Services[i].Distance;
                    property.Services[i].StarRating = model.Services[i].StarRating;
                }
                await _propertyRepository.Save();

                property.Amenities.Clear();
                await _propertyAmenityRepository.Save();
                await AddPropertyAmenities(property.Id, model.Amenities);
                await AddPropertyAmenities(property.Id, model.Safeties);

                if (model.PrimaryImage != null)
                {
                    await _imageRepository.DeletePrimaryImageForProperty(property.Id);
                    Image primaryImage = new Image()
                    {
                        Path = uploadImage(model.PrimaryImage),
                        IsPrimary = true,
                        PropertyId = property.Id
                    };
                    await _imageRepository.Add(primaryImage);
                    await _imageRepository.Save();
                }

                if (model.Images != null)
                {
                    await _imageRepository.DeleteNonPrimaryImagesForProperty(property.Id);

                    foreach (var image in model.Images)
                    {
                        Image img = new Image()
                        {
                            Path = uploadImage(image),
                            IsPrimary = false,
                            PropertyId = property.Id
                        };
                        await _imageRepository.Add(img);
                    }
                    await _imageRepository.Save();
                }

                await _propertyRepository.CommitAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                await _propertyRepository.RollbackAsync();
                return Json($"{ex.Message}, {ex.InnerException}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddProperty()
        {
            var HostAddPropertyViewModel = new HostAddPropertyViewModel();
            await PopulateHostAddPropertyViewModelAsync(HostAddPropertyViewModel);
            return View(HostAddPropertyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty(HostAddPropertyViewModel model)
        {
            if (!ModelState.IsValid) {
                await PopulateHostAddPropertyViewModelAsync(model);
                return View(model);
            }
            try
            {
                await _propertyRepository.BeginTransactionAsync();

                Location loc = new Location()
                {
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Country = model.CountryCode,
                    City = "Dummy",
                    State = model.StateCode,
                };
                await _locationRepository.Add(loc); // 
                await _locationRepository.Save();

                var (unListDate, isFeatured) = CalculateListingDetails(model.ListingPlan);

                Property property = new Property()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Address = model.Address,
                    BedRooms = model.Bedrooms,
                    BathRooms = model.Bathrooms,
                    GarageSlots = model.GarageSlots,
                    BetsAllowd = model.BetsAllowed,
                    Area = model.Area,
                    IsListed = true,
                    IsFeatured = isFeatured,
                    FeesPerMonth = model.FeesPerMonth,
                    ListedAt = DateTime.Now,
                    UnListDate = unListDate,
                    ListingType = (ListingType)model.ListingType,
                    PropertyTypeId = model.PropertyTypeId,
                    UserId = "23d1c943-494f-489b-acaf-5144c2fe2387",
                };
                await _propertyRepository.Add(property);
                await _propertyRepository.Save();

                foreach(var modelService in model.Services)
                {
                    var service = new Service()
                    {
                        Name = modelService.Name,
                        Distance = modelService.Distance,
                        StarRating = modelService.StarRating,
                        PropertyId = property.Id,
                    };
                    await _serviceRepository.Add(service);
                    await _serviceRepository.Save();
                }

                await AddPropertyAmenities(property.Id, model.Amenities);
                await AddPropertyAmenities(property.Id, model.Safeties);

                Image primaryImage = new Image()
                {
                    Path = uploadImage(model.PrimaryImage),
                    IsPrimary = true,
                    PropertyId = property.Id
                };
                await _imageRepository.Add(primaryImage);
                await _imageRepository.Save();

                foreach (var image in model.Images)
                {
                    Image img = new Image()
                    {
                        Path = uploadImage(image),
                        IsPrimary = false,
                        PropertyId = property.Id
                    };
                    await _imageRepository.Add(img);
                    await _imageRepository.Save();
                }

                await _propertyRepository.CommitAsync();
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex) 
            {
                await _propertyRepository.RollbackAsync();
                return Json($"{ex.Message}, {ex.InnerException}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryRepository.GetAll();
            return Json(countries);
        }

        private async Task PopulateHostAddPropertyViewModelAsync(HostAddPropertyViewModel viewModel)
        {
            var propertyTypes = await _propertyTypeRepository.GetAll();
            var amenities = await _amenityRepository.GetAmenities();
            var safeties = await _amenityRepository.GetSafeties();
            var countries = await _countryRepository.GetAll();
            var listingTypes = Enum.GetValues(typeof(ListingType))
                      .Cast<ListingType>()
                      .Select(e => new ListingTypeViewModel
                      {
                          Id = ((int)e),
                          Name = e.ToString()
                      });

            viewModel.propertyTypes = propertyTypes;
            viewModel.amenities = amenities;
            viewModel.safeties = safeties;
            viewModel.countries = countries;
            viewModel.listingTypes = listingTypes;
            if (viewModel.Services == null)
            {
                viewModel.Services = new List<ServiceViewModel>()
                {
                    new ServiceViewModel(),
                    new ServiceViewModel()
                };
            }
        }

        private async Task PopulateHostEditPropertyViewModelAsync(HostEditPropertyViewModel viewModel) {
            var propertyTypes = await _propertyTypeRepository.GetAll();
            var amenities = await _amenityRepository.GetAmenities();
            var safeties = await _amenityRepository.GetSafeties();
            var countries = await _countryRepository.GetAll();
            var listingTypes = Enum.GetValues(typeof(ListingType))
                      .Cast<ListingType>()
                      .Select(e => new ListingTypeViewModel
                      {
                          Id = ((int)e),
                          Name = e.ToString()
                      });

            viewModel.propertyTypes = propertyTypes;
            viewModel.amenities = amenities;
            viewModel.safeties = safeties;
            viewModel.countries = countries;
            viewModel.listingTypes = listingTypes;
            if (viewModel.Services == null)
            {
                viewModel.Services = new List<ServiceViewModel>()
                {
                    new ServiceViewModel(),
                    new ServiceViewModel()
                };
            }
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

        private (DateTime unListDate, bool isFeatured) CalculateListingDetails(int listingPlan)
        {
            var unListDate = DateTime.Now.AddDays(15);
            var isFeatured = false;

            if (listingPlan == 2 || listingPlan == 4)
            {
                unListDate = unListDate.AddDays(15); 
            }
            if (listingPlan == 3 || listingPlan == 4)
            {
                isFeatured = true;
            }
            return (unListDate, isFeatured);
        }

        private async Task AddPropertyAmenities(int propertyId, IEnumerable<int>? amenityIds)
        {
            if (amenityIds == null) return;

            foreach (var amenityId in amenityIds)
            {
                PropertyAmenity propertyAmenity = new PropertyAmenity()
                {
                    PropertyId = propertyId,
                    AmenityId = amenityId
                };
                await _propertyAmenityRepository.Add(propertyAmenity);
            }
             await _propertyAmenityRepository.Save();

        }

        //public async Task<IActionResult> create()
        //{
        //    try
        //    {
        //        var user = new User
        //        {
        //            UserName = "AhmedHamdy",
        //            Email = "ahmed@gmail.com",
        //            Gender = Gender.Male,
        //        };
        //        var result = await _userManager.CreateAsync(user, "Ahmed$1");
        //        var role = await _roleManager.FindByIdAsync("3");
        //        var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
        //        return Json("Success");
        //    }
        //    catch
        //    {
        //        return Json("Fail");
        //    }
        //}

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
            var user =  await _userManager.FindByIdAsync(id);

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
        public async Task<IActionResult> SaveEdit(UserProfileEditViewModel UserFromRequest)
        {
            var user = new User 
            { 
            Id = UserFromRequest.Id,
            Name = UserFromRequest.Name,
            Email = UserFromRequest.Email,
            Image = UserFromRequest.Image,
            Gender = UserFromRequest.Gender,
            PhoneNumber = UserFromRequest.PhoneNumber,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.Update(user);
                    await _userRepository.Save();
                    return RedirectToAction("Profile", new { id = user.Id });
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", message);
                }

            }
            return View("EditProfile", UserFromRequest);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(string id, IFormFile ImgUrl)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && ImgUrl != null)
            {
                string fileName = uploadImage(ImgUrl);
                user.Image = fileName;

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("EditProfile", new { id = id });
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
