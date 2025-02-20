using Microsoft.AspNetCore.Identity;

namespace TransportManagement.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}