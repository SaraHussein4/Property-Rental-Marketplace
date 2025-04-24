using Microsoft.AspNetCore.Identity;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyDAL.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }    
        public Gender? Gender { get; set; }
        public string? Image { get; set; }
        public bool IsAgree {  get; set; }  

        // Navigation Property
        public virtual ICollection<Favourite>? Favourites { get; set; } // For User
        public virtual ICollection<Booking>? BookingsUser { get; set; } // FOR Both Usee And Host
        public virtual ICollection<Booking>? BookingsHost { get; set; } // FOR Both Usee And Host

        public virtual ICollection<Property>? HostedProperties { get; set; }  // For Host
        public virtual ICollection<Notification>? Notifications { get; set; } // For Both

    }
}
