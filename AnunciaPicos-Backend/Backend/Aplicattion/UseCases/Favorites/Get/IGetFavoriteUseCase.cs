using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get
{
    public interface IGetFavoriteUseCase
    {
        public Task<List<ResponseProductGetCommunication>> Execute();
    }
}
