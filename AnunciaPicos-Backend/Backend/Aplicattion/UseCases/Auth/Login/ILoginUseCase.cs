using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        public Task<ResponseLoginCommunication> Execute(RequestLoginCommunication request);
        public void ValidateUser(RequestLoginCommunication request);
        public Task<ResponseLoginCommunication> VerifyLoginCredential(RequestLoginCommunication request);

    }
}
