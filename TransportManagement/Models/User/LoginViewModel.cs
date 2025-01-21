using System.ComponentModel.DataAnnotations;

namespace TransportManagement.Models.User
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
