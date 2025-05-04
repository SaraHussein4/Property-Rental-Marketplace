using System.ComponentModel.DataAnnotations;
using PropertyRentalDAL.Enumerates;

namespace PropertyRentalMarketplace.ViewModels
{
    public class UserProfileEditViewModel
    {
        public Gender? Gender { get; set; }
        public string? Image { get; set; }
        public IFormFile ImgUrl { get; set; }

        [RegularExpression(@"^(011|012|015|010)\d{8}$",
        ErrorMessage = "Phone number is invalid !")]
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Id { get; set; }
        public string? Password { get; set; }


    }
}
