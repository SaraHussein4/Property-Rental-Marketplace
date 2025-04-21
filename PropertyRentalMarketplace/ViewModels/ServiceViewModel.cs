using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class ServiceViewModel
    {
        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Distance is required")]
        [Range(0, 10000, ErrorMessage = "Distance must be between 0 and 10,000 meters")]
        public int Distance { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public int StarRating { get; set; }
    }
}
