using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Profile;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update
{
    public interface IUpdateUserUseCase
    {
        public Task Execute(RequestUpdateProfileCommunication request);

        public Task ValidateUser(RequestUpdateProfileCommunication request, UserModel user);

    }
}
