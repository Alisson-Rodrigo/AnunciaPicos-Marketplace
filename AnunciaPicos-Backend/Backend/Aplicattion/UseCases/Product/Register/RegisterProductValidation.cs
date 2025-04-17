using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Exceptions;
using FluentValidation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Register
{
    public class RegisterProductValidation : AbstractValidator<RequestRegisterProductCommunication>
    {
        public RegisterProductValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_PRODUCT_EMPTY);
            RuleFor(x => x.Description).NotEmpty().WithMessage(ResourceMessagesException.DESCRIPTION_PRODUCT_EMPTY);
            RuleFor(x => x.Price).NotEmpty().WithMessage(ResourceMessagesException.PRICE_PRODUCT_EMPTY);
            RuleFor(x => x.Categories).NotEmpty().WithMessage(ResourceMessagesException.CATEGORY_PRODUCT_EMPTY);
        }
    }
}
