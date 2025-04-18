using PropertyDAL.Models;
using PropertyRentalDAL.Enumerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Property
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

        // Navigation Properties
        public virtual PropertyType PropertyType { get; set; } //
        public virtual Location Location { get; set; } //
        public virtual User Host { get; set; } //
        public virtual ICollection<PropertyService> Services { get; set; } //
        public virtual ICollection<PropertyAmenity> Amenities { get; set; } //
        public virtual ICollection<Image> Images { get; set; } //
        public virtual ICollection<Favourite> Favourites { get; set; } //
        public virtual ICollection<Booking> Bookings { get; set; } //


    }
}
