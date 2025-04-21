using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public int StarRating { get; set; }

        // Foreign Key
        public int PropertyId { get; set; }

        // Navigation Properties
        public virtual Property Property { get; set; }
        //public virtual ICollection<PropertyService> Properties { get; set; } //
    }
}
