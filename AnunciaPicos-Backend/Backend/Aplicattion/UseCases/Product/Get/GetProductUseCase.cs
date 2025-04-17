using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Response.Product;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Get
{
    public class GetProductUseCase : IGetProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductUseCase(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ResponseProductGetCommunication>> Execute()
        {
            var response = await _productRepository.GetProducts();

            if (response == null || !response.Any())
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }

            var products = _mapper.Map<List<ResponseProductGetCommunication>>(response);

            // Nesse ponto, `products` já vai conter a lista de imagens de cada produto
            return products;
        }

    }
}
