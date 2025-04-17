using AnunciaPicos.Shared.Communication.Request.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword
{
    public interface IResetUseCase
    {
        public Task Execute(RequestResetPasswordCommunication request);

    }
}
