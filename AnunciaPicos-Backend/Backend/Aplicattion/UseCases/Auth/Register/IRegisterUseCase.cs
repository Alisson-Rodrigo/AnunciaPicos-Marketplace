using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register
{
    public interface IRegisterUseCase
    {
        public Task<ResponseRegisterCommunication> Execute(RequestRegisterCommunication request);
        public Task ValidateUser(RequestRegisterCommunication request);
    }
}
