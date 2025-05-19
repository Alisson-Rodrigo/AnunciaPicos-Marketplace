using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get
{
    public interface IGetFavoriteUseCase
    {
        public Task<List<ProductModel>> Execute();
    }
}
