using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class PropertyService
    {
        // Composite primary key (Foreign keys)
        public int PropertyId { get; set; }
        public int ServiceId { get; set; }

        // Navigation Properties
        public virtual Property Property { get; set; }
        public virtual Service Service { get; set; }
    }
}
