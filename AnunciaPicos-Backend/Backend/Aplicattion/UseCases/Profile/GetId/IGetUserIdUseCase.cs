using AnunciaPicos.Shared.Communication.Response.Profile;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.GetId
{
    public interface IGetUserIdUseCase
    {
        public Task<ResponseGetProfileIdCommunication> Execute(int id);
    }
}
