using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Favourite
    {
        // Composite primary key (Foreign keys)
        public string UserId { get; set; }
        public int PropertyId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } // 
        public virtual Property Property { get; set; } //
    }
}
