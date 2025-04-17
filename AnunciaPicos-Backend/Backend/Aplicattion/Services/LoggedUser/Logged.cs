using AnunciaPicos.Backend.Aplicattion.Services.TokenLogin;
using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace AnunciaPicos.Backend.Aplicattion.Services.LoggedUser
{
    public class Logged : ILogged
    {
        private readonly AnunciaPicosDbContext _dbContext;
        private readonly GetTokenRequest _getTokenRequest;

        public Logged(AnunciaPicosDbContext dbContext, GetTokenRequest getTokenRequest)
        {
            _dbContext = dbContext;
            _getTokenRequest = getTokenRequest;
        }

        public async Task<UserModel> UserLogged()
        {
            var token = _getTokenRequest.GetToken();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurity = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurity.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var userIdentifier = Guid.Parse(identifier);

            return await _dbContext.Users
                .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
        }

    }
}

