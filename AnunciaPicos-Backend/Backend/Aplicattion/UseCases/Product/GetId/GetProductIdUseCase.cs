using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Response.Product;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.GetId
{
    public class GetProductIdUseCase : IGetProductIdUseCase
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductIdUseCase(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<ResponseProductGetCommunication> Execute(int id)
        {
            var response = await _productRepository.GetProductId(id);
            if (response == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }
            var product = _mapper.Map<ResponseProductGetCommunication>(response);
            return product;
        }

    }
}
