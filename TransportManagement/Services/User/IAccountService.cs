using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public interface IAccountService
    {
        Task<ApplicationUser> LoginUserAsync(LoginViewModel model);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task LogoutAsync();
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task SendPasswordResetEmailAsync(string email, string resetUrlBase);
    }
}