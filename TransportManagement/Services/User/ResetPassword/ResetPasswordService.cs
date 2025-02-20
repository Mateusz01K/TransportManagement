using Microsoft.AspNetCore.Identity;
using TransportManagement.Models.User;
using TransportManagement.Services.User.EmailSender;

namespace TransportManagement.Services.User.ResetPassword
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task SendPasswordResetEmailAsync(string email, string resetUrlBase)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{resetUrlBase}?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(email, "Reset hasła",
                $"Aby zresetować hasło, kliknij w poniższy link: <a href='{resetUrl}'>Resetuj hasło.</a>");
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;

        }
    }
}
