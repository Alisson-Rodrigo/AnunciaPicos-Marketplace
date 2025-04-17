using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Exceptions;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Update
{
    public class UpdateProductUseCase : IUpdateProductUseCase
    {

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogged _logged;

        public UpdateProductUseCase(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogged logged)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logged = logged;
        }


        public async Task Execute(RequestUpdateProductCommunication request, int id)
        {
            ValidateProduct(request);

            var product = await _productRepository.GetProductId(id);
            var user = await _logged.UserLogged();

            if (product == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }

            if (product.UserId != user.Id)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_ALLOWED);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Category = request.Categories;
            product.UpdatedAt = DateTime.Now;

            _productRepository.UpdateProduct(product);
            await _unitOfWork.Commit();

        }

        public void ValidateProduct (RequestUpdateProductCommunication request)
        {
            var validator = new UpdateProductValidation();
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }

    }
}
