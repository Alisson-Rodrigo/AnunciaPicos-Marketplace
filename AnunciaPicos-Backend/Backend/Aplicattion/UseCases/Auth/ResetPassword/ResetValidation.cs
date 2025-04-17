using AnunciaPicos.Shared.Communication.Request.Auth;
using FluentValidation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword
{
    public class ResetValidation : AbstractValidator<RequestResetPasswordCommunication>
    {
        public ResetValidation()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token não pode ser vazio.")
                .NotNull().WithMessage("Token não pode ser nulo.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha não pode ser vazia.")
                .NotNull().WithMessage("Senha não pode ser nula.")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirmação de senha não pode ser vazia.")
                .NotNull().WithMessage("Confirmação de senha não pode ser nula.")
                .Equal(x => x.Password).WithMessage("Senha e confirmação não conferem.");
            
        }
    }
}
