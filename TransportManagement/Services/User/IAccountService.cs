using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public interface IAccountService
    {
        Task<ApplicationUser> LoginUserAsync(LoginViewModel model);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confimPassword);
        Task LogoutAsync();
    }
}