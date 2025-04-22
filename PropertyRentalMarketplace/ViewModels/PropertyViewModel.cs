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
        

        // Foreign Key
        public int PropertyTypeId { get; set; }
        public int LocationId { get; set; }
        public string UserId { get; set; }
        public bool IsFavourite { get; set; }

        // Navigation Properties
        public virtual PropertyType PropertyType { get; set; } //
        public virtual Location Location { get; set; } //
        public virtual User Host { get; set; } //
        public IEnumerable<PropertyService> Services { get; set; } //
        public IEnumerable<PropertyAmenity> Amenities { get; set; } //
        public IEnumerable<Image> Images { get; set; } //
        public IEnumerable<Favourite> Favourites { get; set; } //
        public IEnumerable<Booking> Bookings { get; set; } //
    }
}
