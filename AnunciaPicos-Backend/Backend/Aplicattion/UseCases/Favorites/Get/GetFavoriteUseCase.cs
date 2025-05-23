using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Response.Product;
using AutoMapper;
using Azure;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get
{
    public class GetFavoriteUseCase : IGetFavoriteUseCase
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogged _logged;
        public GetFavoriteUseCase(IFavoriteRepository favoriteRepository, ILogged logged, IProductRepository productRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _logged = logged;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<List<ResponseProductGetCommunication>> Execute()
        {
            UserModel user = await _logged.UserLogged();

            var favorites = await _favoriteRepository.GetFavorites(user.Id);

            if (favorites == null || !favorites.Any())
            {
                throw new AnunciaPicosExceptions("Nenhum produto favoritado.");
            }

            List<ProductModel> productsFavorites = await _productRepository.GetProductsFavorites(favorites);

            var products = _mapper.Map<List<ResponseProductGetCommunication>>(productsFavorites);

            return products;
        }
    }

}
