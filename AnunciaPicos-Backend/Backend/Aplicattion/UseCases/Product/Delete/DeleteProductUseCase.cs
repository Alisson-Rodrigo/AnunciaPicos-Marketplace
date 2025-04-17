using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Exceptions;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Delete
{
    public class DeleteProductUseCase : IDeleteProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogged _logged;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductUseCase(IProductRepository productRepository, ILogged logged, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _logged = logged;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int id)
        {
            var user = await _logged.UserLogged();
            var product = await _productRepository.GetProductId(id);
            if (product == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }

            if (user.Id != product.UserId)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_ALLOWED);
            }

            _productRepository.DeleteProduct(product);

            await _unitOfWork.Commit();
        }
    }
}
