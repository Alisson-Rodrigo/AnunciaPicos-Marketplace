using AnunciaPicos.Backend.Aplicattion.Services.SMTPEmail;
using AnunciaPicos.Backend.Aplicattion.Services.TokenUpdatePassword;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Exceptions;


namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword
{
    public class UpdatePasswordUseCase : IUpdatePasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly ISend _send;
        private IConfiguration _configuration;

        public UpdatePasswordUseCase(IUserRepository userRepository, IPasswordResetTokenService passwordResetTokenService, ISend send, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordResetTokenService = passwordResetTokenService;
            _send = send;
            _configuration = configuration;
        }

        public async Task Execute(RequestUpdatePasswordCommunication request)
        {
            await ValidateEmail(request); //ok
            await SendLinkEmail(request); //ok
        }

        public async Task ValidateEmail(RequestUpdatePasswordCommunication request)
        {
            //realizar a verificação do email enviado, se é um email válido
            var validator = new UpdatePasswordValidation();
            var response = validator.Validate(request);
            //realizar a verificação se o email existe
            var responseEmailExists = await _userRepository.VerifyEmailExists(request.Email);
            if (responseEmailExists == false)
            {
                response.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_NOT_EXISTING));
            }
            if (response.IsValid == false)
            {
                var errorMessages = response.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }

        public async Task SendLinkEmail(RequestUpdatePasswordCommunication request)
        {
            // Busca o usuário pelo e-mail
            var user = await _userRepository.GetByUserEmail(request.Email);
            if (user == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }

            // Gera o token de recuperação
            var token = _passwordResetTokenService.GeneratePasswordResetToken(user);

            // Obtém a URL base para recuperação (definida em appsettings.json, por exemplo em "AppSettings:PasswordResetUrl")
            var baseUrl = _configuration.GetValue<string>("AppSettings:PasswordResetUrl");
            // Constrói o link de recuperação (ex: https://www.seusite.com/resetpassword?token=...)
            var recoveryLink = $"{baseUrl}?token={token}";

            // Envia o e-mail com o link de recuperação. 
            // Atenção: o método SendRecoveryEmail espera primeiro o e-mail do destinatário e depois o link.
            var responseSend = _send.SendRecoveryEmail(request.Email.Trim(), recoveryLink);
        }


    }
}
