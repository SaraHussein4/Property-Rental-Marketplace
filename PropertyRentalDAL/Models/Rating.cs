using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int OverallRating { get; set; }
        public int AmenitiesRating { get; set; }
        public int CommunicationRating { get; set; }
        public int ValueForMoneyRating { get; set; }
        public int HygieneRating { get; set; }
        public int LocationRating { get; set; }

        // Navigation Property
        public virtual Booking Booking { get; set; } //

    }
}
