using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AnunciaPicos.Backend.Aplicattion.Services.TokenUpdatePassword
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        private readonly IConfiguration _configuration;

        public PasswordResetTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para gerar o token (conforme mostrado acima)
        public string GeneratePasswordResetToken(UserModel user)
        {
            // Obtém as configurações JWT do appsettings.json
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            // Defina um tempo curto para expiração (por exemplo, 15 minutos)
            var expires = 15;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Cria os claims necessários, incluindo um claim de propósito
            var claims = new[]
            {
                new Claim("purpose", "password_reset"),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Método para validar o token de recuperação de senha
        public bool ValidatePasswordResetToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings.GetValue<string>("SecretKey");
                var issuer = jwtSettings.GetValue<string>("Issuer");
                var audience = jwtSettings.GetValue<string>("Audience");

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Verifica se o token possui o claim "purpose" com valor "password_reset"
                if (!principal.HasClaim(c => c.Type == "purpose" && c.Value == "password_reset"))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
