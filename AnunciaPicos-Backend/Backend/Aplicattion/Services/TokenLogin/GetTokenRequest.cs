using Org.BouncyCastle.Pqc.Crypto.Saber;

namespace AnunciaPicos.Backend.Aplicattion.Services.TokenLogin
{
    public class GetTokenRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTokenRequest(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return authentication["Bearer ".Length..].Trim();
        }


    }
}
