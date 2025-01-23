using Microsoft.AspNetCore.Identity;

namespace TransportManagement.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}