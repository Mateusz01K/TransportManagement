namespace TransportManagement.Services.User.RoleService
{
    public interface IRoleService
    {
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<bool> UnAssignRoleAsync(string userId, string roleName);
    }
}
