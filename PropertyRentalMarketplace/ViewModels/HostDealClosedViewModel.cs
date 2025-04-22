using PropertyDAL.Models;
using PropertyRentalDAL.Models;

namespace PropertyRentalMarketplace.ViewModels
{
    public class HostDealClosedViewModel
    {
        public int PropertyId { get; set; }
        public int FeePerMonth { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string phoneNumber { get; set; }

    }
}
