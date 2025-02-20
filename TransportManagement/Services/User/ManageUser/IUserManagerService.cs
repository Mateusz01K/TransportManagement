using TransportManagement.Models.User;

namespace TransportManagement.Services.User.ManageUser
{
    public interface IUserManagerService
    {
        Task<List<UserDto>> GetUserAsync();
    }
}
