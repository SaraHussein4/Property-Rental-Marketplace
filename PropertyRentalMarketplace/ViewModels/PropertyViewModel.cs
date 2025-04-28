using PropertyDAL.Models;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int GarageSlots { get; set; }
        public int BetsAllowd { get; set; }
        public int Area { get; set; }
        public bool IsListed { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime ListedAt { get; set; }
        public DateTime UnListDate { get; set; }
        public ListingType ListingType { get; set; }

        public int FeesPerMonth { get; set; }
        //data host
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        // Foreign Key
        public int PropertyTypeId { get; set; }
        public int LocationId { get; set; }
        public string UserId { get; set; }
        public bool IsFavourite { get; set; }
        //image list
        public List<Image> Images { get; set; }
        public Property Property { get; set; }
        //Amenities 
        public List<Amenity> amenities {  get; set; }
        public List<Amenity>? safeties { get; set; }
        //services
        public List<Service> services { get; set; }
        //fav

        //map

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Navigation Properties
        public virtual PropertyType PropertyType { get; set; } //
        public virtual Location Location { get; set; } //
        public virtual User Host { get; set; } //
        public IEnumerable<PropertyService> Services { get; set; } //
        public IEnumerable<PropertyAmenity> Amenities { get; set; } //
        //public IEnumerable<Image> Images { get; set; } //
        public List<IFormFile> Imageshost { get; set; }
        public string ImagesHost { get; set; }
        public IEnumerable<Favourite> Favourites { get; set; } //
        public IEnumerable<Booking> Bookings { get; set; } //
    }
}
