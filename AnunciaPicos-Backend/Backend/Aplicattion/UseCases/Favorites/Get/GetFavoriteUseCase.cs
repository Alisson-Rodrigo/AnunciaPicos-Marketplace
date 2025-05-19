using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Exceptions.ExceptionBase;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get
{
    public class GetFavoriteUseCase : IGetFavoriteUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogged _logged;
        public GetFavoriteUseCase(IFavoriteRepository favoriteRepository, ILogged logged, IProductRepository productRepository)
        {
            _favoriteRepository = favoriteRepository;
            _logged = logged;
            _productRepository = productRepository;
        }
        public async Task<List<ProductModel>> Execute()
        {
            UserModel user = await _logged.UserLogged();

            var favorites = await _favoriteRepository.GetFavorites(user.Id);

            if (favorites == null || !favorites.Any())
            {
                throw new AnunciaPicosExceptions("Nenhum produto favoritado.");
            }

            var productsFavorites = await _productRepository.GetProductsFavorites(favorites);

            return productsFavorites;
        }
    }

}
