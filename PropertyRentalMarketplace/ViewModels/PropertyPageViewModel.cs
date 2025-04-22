using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class PropertyPageViewModel
    {
        public IEnumerable<Property>? AllProperities { get; set; }
        public IEnumerable<Property>? FeaturedProperities { get; set; }

    }
}
