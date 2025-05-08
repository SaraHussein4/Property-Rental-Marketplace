using PropertyRentalDAL.Enumerates;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class RegisterViewModel
    {
            [Required(ErrorMessage = "Name Is Required")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email Is Required")]
            [EmailAddress(ErrorMessage = "Invalid Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password Is Required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm Password Is Required")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password Doesn't Match")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Gender Is Required")]
            public Gender? Gender { get; set; }
           [Required(ErrorMessage = "Agree Is Required")]

        public bool IsAgree { get; set; }
        

    }
}
