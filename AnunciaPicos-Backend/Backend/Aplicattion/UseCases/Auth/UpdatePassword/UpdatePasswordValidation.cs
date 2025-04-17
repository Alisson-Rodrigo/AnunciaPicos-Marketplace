using AnunciaPicos.Exceptions;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Exceptions;
using FluentValidation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword
{
    public class UpdatePasswordValidation : AbstractValidator<RequestUpdatePasswordCommunication>
    {
        public UpdatePasswordValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        }
    }
}
