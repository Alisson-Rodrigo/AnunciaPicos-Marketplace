using AnunciaPicos.Exceptions;
using FluentValidation;
using AnunciaPicos.Backend.Aplicattion.Services.ValidatorCpf;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Exceptions;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register
{
    public class RegisterUserValidation : AbstractValidator<RequestRegisterCommunication>
    {
        private readonly ICPF _cpf;

        public RegisterUserValidation(ICPF cpf)
        {
            _cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));

            // Validação para Username:
            RuleFor(x => x.Apelido)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                // Verifica se o valor não possui espaços no início ou no fim:
                .Must(username => username == username.Trim())
                    .WithMessage(ResourceMessagesException.NAME_INVALID)
                // Garante que não haja espaços internos:
                .Must(username => !username.Contains(" "))
                    .WithMessage(ResourceMessagesException.NAME_INVALID);

            // Validação para Email:
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID)
                // Verifica se o email não possui espaços no início ou no fim:
                .Must(email => email == email.Trim())
                    .WithMessage(ResourceMessagesException.EMAIL_INVALID);

            // Validação para Password (sem transformações):
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ResourceMessagesException.PASSWORD_EMPTY)
                .MinimumLength(6).WithMessage(ResourceMessagesException.PASSWORD_MINIUM);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(ResourceMessagesException.PASSWORD_CONFIRM);

            // Validação para CPF:
            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage(ResourceMessagesException.CPF_EMPTY)
                .Must(cpf => cpf == cpf.Trim()).WithMessage(ResourceMessagesException.CPF_INVALID)
                .Must(cpf => !cpf.Contains(" ")).WithMessage(ResourceMessagesException.CPF_INVALID)
                .Must(_cpf.IsValidCPF).WithMessage(ResourceMessagesException.CPF_INVALID);

            RuleFor(x => x.Phone).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
            RuleFor(x => x.Phone).Must(phone => phone == phone.Trim()).WithMessage(ResourceMessagesException.PHONE_INVALID);
            RuleFor(x => x.Phone).Must(phone => !phone.Contains(" ")).WithMessage(ResourceMessagesException.PHONE_INVALID);

        }
    }
}
