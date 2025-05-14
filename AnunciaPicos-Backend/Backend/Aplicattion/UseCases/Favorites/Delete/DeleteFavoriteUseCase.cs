using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Exceptions;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Delete
{
    public class DeleteFavoriteUseCase : IDeleteFavoriteUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogged _logged;

        public DeleteFavoriteUseCase(IFavoriteRepository favoriteRepository, IUnitOfWork unitOfWork, ILogged logged)
        {
            _favoriteRepository = favoriteRepository;
            _unitOfWork = unitOfWork;
            _logged = logged;
        }

        public async Task Execute(int productId)
        {
            UserModel user = await _logged.UserLogged();

            var favorite = await _favoriteRepository.GetFavoriteByUserIdAndProductId(user.Id, productId);

            if (user.Id != favorite!.UserId)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_ALLOWED);
            }

            if (favorite == null)
            {
                throw new Exception("Favorito não encontrado");
            }
            _favoriteRepository.Remove(favorite);
            await _unitOfWork.Commit();


        }
    }
}
