using System.Security.Claims;
using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal user);
        Task<Dictionary<string, string>> GetAllUsersEmails();
    }
}
