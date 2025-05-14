using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get
{
    public class GetFavoriteUseCase : IGetFavoriteUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ILogged _logged;
        public GetFavoriteUseCase(IFavoriteRepository favoriteRepository, ILogged logged)
        {
            _favoriteRepository = favoriteRepository;
            _logged = logged;
        }
        public async Task Execute()
        {

            var favorites = await _favoriteRepository.GetFavorites();
        }
    }

}
