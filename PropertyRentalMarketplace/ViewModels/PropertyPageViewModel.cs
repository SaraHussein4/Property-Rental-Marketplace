using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class PropertyPageViewModel
    {
        public List<PropertyViewModel>? AllProperities  { get; set; }
        public List<PropertyViewModel>? FeaturedProperities { get; set; }

    }
}
