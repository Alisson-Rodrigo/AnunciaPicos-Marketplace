﻿using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Exceptions;

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
            var user = await _logged.UserLogged();

            if (user == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }

            var favoriteExists = await _favoriteRepository.GetFavoriteByUserIdAndProductId(user.Id, productId);

            if (favoriteExists != null)
            {
                throw new AnunciaPicosExceptions("Produto já favoritado.");
            }

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
