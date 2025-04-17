using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Exceptions;
using FluentValidation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login
{
    public class LoginValidation : AbstractValidator<RequestLoginCommunication>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(x => x.Password).NotEmpty().WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
        }
    }
}
