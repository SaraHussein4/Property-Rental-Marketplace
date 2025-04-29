using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class FindPropertyViewModel
    {
        public List<PropertyType> PropertyTypes { get; set; }
        public List<Property> InitialProperties { get; set; }

        
    }
}
