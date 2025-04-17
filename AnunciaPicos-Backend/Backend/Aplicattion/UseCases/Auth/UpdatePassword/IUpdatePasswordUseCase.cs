using AnunciaPicos.Shared.Communication.Request.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword
{
    public interface IUpdatePasswordUseCase
    {
        public Task Execute(RequestUpdatePasswordCommunication request);
    }
}
