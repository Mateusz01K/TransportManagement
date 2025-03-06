using System.ComponentModel.DataAnnotations;

namespace TransportManagement.Models.User
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set;}
    }
}
