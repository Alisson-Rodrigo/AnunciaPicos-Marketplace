using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Register
{
    public class RegisterFavoriteUseCase : IRegisterFavoriteUseCase
    {
        private readonly ILogged _logged;
        private readonly IUnitOfWork _unitOfWord;
        private readonly IFavoriteRepository _favoriteRepository;
        public RegisterFavoriteUseCase(ILogged logged, IUnitOfWork unitOfWord, IFavoriteRepository favoriteRepository)
        {
            _logged = logged;
            _favoriteRepository = favoriteRepository;
            _unitOfWord = unitOfWord;
        }
        public async Task Execute(int productId)
        {
            var user = _logged.UserLogged();
            var favorite = new FavoriteModel
            {
                UserId = user.Id,
                ProductId = productId
            };
            await _favoriteRepository.Add(favorite);
            await _unitOfWord.Commit();
        }

    }
}
