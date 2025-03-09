using TransportManagement.Models.User;

namespace TransportManagement.Services.User.ManageUser
{
    public interface IUserManagerService
    {
        Task<List<UserDto>> GetUserAsync();
        Task<bool> UpdateUserAsync(string Email, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Address, int Experience, decimal Salary);
        Task<bool> DeleteUserAsync(string email);
    }
}
