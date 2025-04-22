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
        public virtual ICollection<Favourite>? Favourites { get; set; } // 
        public virtual ICollection<Booking>? Bookings { get; set; } //
        public virtual ICollection<Property>? HostedProperties { get; set; }  //
        public virtual ICollection<Notification>? Notifications { get; set; } //

    }
}
