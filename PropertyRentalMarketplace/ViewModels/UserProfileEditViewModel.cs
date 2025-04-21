using PropertyRentalDAL.Enumerates;

namespace PropertyRentalMarketplace.ViewModels
{
    public class UserProfileEditViewModel
    {
        public Gender? Gender { get; set; }
        public string? Image { get; set; }
        public IFormFile ImgUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Id { get; set; }
        public string? Password { get; set; }


    }
}
