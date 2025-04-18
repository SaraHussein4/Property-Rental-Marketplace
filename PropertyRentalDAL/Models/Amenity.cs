using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }

        // Foreign Key
        public int AmenityCategoryId { get; set; }

        // Navigation Properties
        public virtual ICollection<PropertyAmenity> Properties { get; set; } //
        public virtual AmenityCategory AmenityCategory { get; set; } // 
    }
}
