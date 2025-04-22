using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required (ErrorMessage ="New Password Is Required")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm New Password Is Required")]
        [Compare("NewPassword" ,ErrorMessage ="Password Doesn't Match")]
        public string ConfirmNewPassword { get; set; }
    }
}
