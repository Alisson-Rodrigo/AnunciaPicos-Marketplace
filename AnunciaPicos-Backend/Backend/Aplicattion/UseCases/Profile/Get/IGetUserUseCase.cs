using AnunciaPicos.Shared.Communication.Response.Profile;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Get
{
    public interface IGetUserUseCase
    {

        public Task<ResponseGetProfileCommunication> Execute();

    }
}
