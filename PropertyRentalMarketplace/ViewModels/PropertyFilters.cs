namespace PropertyRentalMarketplace.ViewModels
{
    public class PropertyFilters
    {
        public int TypeId { get; set; }
        public List<string> PriceRanges { get; set; } = new List<string>();
        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Bedrooms { get; set; } = new List<string>();
    }
}
