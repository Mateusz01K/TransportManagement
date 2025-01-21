using System.ComponentModel.DataAnnotations;

namespace TransportManagement.Models.User.ResetPassword
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Proszę podać adres e-mail.")]
        [EmailAddress(ErrorMessage = "Proszę podać poprawny adres e-mail.")]
        public string Email { get; set; }
    }
}