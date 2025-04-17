using AnunciaPicos.Exceptions;
using AnunciaPicos.Shared.Communication.Request.Profile;
using AnunciaPicos.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update
{
    public class UpdateUserValidation : AbstractValidator<RequestUpdateProfileCommunication>
    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(x => x.Name).Must(username => username == username.Trim()).WithMessage(ResourceMessagesException.NAME_INVALID);
            RuleFor(x => x.Name).Must(username => !username.Contains(" ")).WithMessage(ResourceMessagesException.NAME_INVALID);
            RuleFor(x => x.Name).MaximumLength(50).WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);
            RuleFor(x => x.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            RuleFor(x => x.Email).Must(email => email == email.Trim()).WithMessage(ResourceMessagesException.EMAIL_INVALID);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
            RuleFor(x => x.Phone).Must(phone => phone == phone.Trim()).WithMessage(ResourceMessagesException.PHONE_INVALID);
            RuleFor(x => x.Phone).Must(phone => !phone.Contains(" ")).WithMessage(ResourceMessagesException.PHONE_INVALID);
        }
    }
}
