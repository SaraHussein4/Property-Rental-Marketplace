using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }

        // Navigation Properities
        public virtual ICollection<Property> Properties { get; set; }//
    }
}
