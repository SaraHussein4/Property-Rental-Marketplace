using Microsoft.AspNetCore.Mvc;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalMarketplace.ViewModels;

namespace PropertyRentalMarketplace.Controllers
{
    public class HostController : Controller
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly ICountryRepository _countryRepository;


        public HostController(IPropertyTypeRepository propertyTypeRepository, IAmenityRepository amenityRepository, ICountryRepository countryRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _amenityRepository = amenityRepository;
            _countryRepository = countryRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddProperty()
        {
            var propertyTypes = await _propertyTypeRepository.GetAll();
            var amenities = await _amenityRepository.GetAmenities();
            var safeties = await _amenityRepository.GetSafeties();
            var countries = await _countryRepository.GetAll();

            var HostAddPropertyViewModel = new HostAddPropertyViewModel()
            {
                propertyTypes = propertyTypes,
                amenities = amenities,
                safeties = safeties,
                countries = countries,

                PropertyTypeId = 4,
                Bedrooms = 0,
                Bathrooms = 0,
                GarageSlots = 0,
                BetsAllowed = 0,
                Amenities = new int[] {1, 2},
                Safeties = new int[] { 7, 11 },

            };
            return View(HostAddPropertyViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty([FromBody] HostAddPropertyViewModel model)
        {
            int x = 1;
            int y = 2;
            return Json(new { success = true, message = "Data saved" });
        }

    }
}
