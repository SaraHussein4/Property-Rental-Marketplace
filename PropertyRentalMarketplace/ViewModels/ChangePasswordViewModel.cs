using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class ChangePasswordViewModel
    {
            public string Id { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string OldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]

        public string NewPassword { get; set; }

            [Required]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]

        public string ConfirmPassword { get; set; }
        }

    }

