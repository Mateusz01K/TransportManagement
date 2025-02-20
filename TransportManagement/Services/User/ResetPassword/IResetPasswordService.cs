namespace TransportManagement.Services.User.ResetPassword
{
    public interface IResetPasswordService
    {
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task SendPasswordResetEmailAsync(string email, string resetUrlBase);
    }
}
