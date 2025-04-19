using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class HostAddPropertyViewModel
    {
        public IEnumerable<PropertyType>? propertyTypes { get; set; }
        public IEnumerable<Amenity>? amenities {  get; set; }
        public IEnumerable<Amenity>? safeties {  get; set; }
        public IEnumerable<Country>? countries {  get; set; }


        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int PropertyTypeId { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int GarageSlots { get; set; }
        public int BetsAllowed { get; set; }
        public int Area { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }



        public IEnumerable<int> Amenities { get; set; }
        public IEnumerable<int> Safeties { get; set; }

    }
}
