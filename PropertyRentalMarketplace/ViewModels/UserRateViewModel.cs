using PropertyDAL.Models;
using PropertyRentalDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class UserRateViewModel
    {
        [Required(ErrorMessage = "OverallRating is required")]
        public int OverallRating { get; set; }
        [Required(ErrorMessage = "AmenitiesRating is required")]
        public int AmenitiesRating { get; set; }
        [Required(ErrorMessage = "CommunicationRating is required")]

        public int CommunicationRating { get; set; }

        [Required(ErrorMessage = "ValueForMoneyRating is required")]
        public int ValueForMoneyRating { get; set; }
        [Required(ErrorMessage = "HygieneRating is required")]

        public int HygieneRating { get; set; }
        [Required(ErrorMessage = "LocationRating is required")]

        public int LocationRating { get; set; }

        [Required(ErrorMessage = "Review is required")]
        [MaxLength(200, ErrorMessage = "Review cannot exceed 200 characters")]
        [MinLength(50, ErrorMessage = "Review cannot be less than 50 characters")]
        public string Review { get; set; }

        public int BookingId { get; set; }
    }
}
