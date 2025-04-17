using AnunciaPicos.Backend.Infrastructure.Models;
using System.Security.Claims;

namespace AnunciaPicos.Backend.Aplicattion.Services.TokenUpdatePassword
{
    public interface IPasswordResetTokenService
    {
        public string GeneratePasswordResetToken(UserModel user);
        public bool ValidatePasswordResetToken(string token, out ClaimsPrincipal principal);


    }
}
