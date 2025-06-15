using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.AuthFacebook
{
    public interface IFacebookLoginUseCase
    {
        Task<ResponseFacebookLoginCommunication> Execute(RequestFacebookLoginCommunication request);
    }
}
