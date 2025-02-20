﻿using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public interface IAccountService
    {
        Task<ApplicationUser> LoginUserAsync(LoginViewModel model);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task LogoutAsync();
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task SendPasswordResetEmailAsync(string email, string resetUrlBase);
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<bool> UnAssignRoleAsync(string userId, string roleName);
        Task<bool> UpdateUserAsync(string Email, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Address, int Experience);
        Task<List<UserDto>> GetUserAsync();
    }
}