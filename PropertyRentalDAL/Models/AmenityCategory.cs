using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class AmenityCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public virtual ICollection<Amenity> Amenities { get; set; } //
    }
}
