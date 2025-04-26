using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class HostParentViewModel
    {
        public IEnumerable<PropertyType>? propertyTypes { get; set; }
        public IEnumerable<Amenity>? amenities { get; set; }
        public IEnumerable<Amenity>? safeties { get; set; }
        public IEnumerable<Country>? countries { get; set; }
        public IEnumerable<ListingTypeViewModel>? listingTypes { get; set; }
        public List<ServiceViewModel> Services { get; set; }

    }
}
