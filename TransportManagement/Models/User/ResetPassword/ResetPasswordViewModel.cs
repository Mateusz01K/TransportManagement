using System.ComponentModel.DataAnnotations;

namespace TransportManagement.Models.User.ResetPassword
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same.")]
        public string ConfirmPassword { get; set; }
    }
}