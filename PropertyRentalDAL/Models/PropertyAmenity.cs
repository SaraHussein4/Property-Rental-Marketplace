using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class PropertyAmenity
    {
        // Composite primary key (Foreign keys)
        public int PropertyId { get; set; }
        public int AmenityId { get; set; }

        // Navigation Properties
        public virtual Property Property { get; set; } //
        public virtual Amenity Amenity { get; set; } //
    }
}
