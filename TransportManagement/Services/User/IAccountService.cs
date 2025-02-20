using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public interface IAccountService
    {
        Task<ApplicationUser> LoginUserAsync(LoginViewModel model);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task LogoutAsync();
    }
}