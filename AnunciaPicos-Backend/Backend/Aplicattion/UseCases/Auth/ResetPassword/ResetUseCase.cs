using AnunciaPicos.Exceptions.ExceptionBase;
using System.Security.Claims;
using AnunciaPicos.Backend.Aplicattion.Services.HashPassword;
using AnunciaPicos.Backend.Aplicattion.Services.TokenUpdatePassword;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Shared.Communication.Request.Auth;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword
{
    public class ResetUseCase : IResetUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ResetUseCase(
            IUserRepository userRepository,
            IPasswordResetTokenService passwordResetTokenService,
            IPasswordEncrypter passwordEncrypter,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordResetTokenService = passwordResetTokenService;
            _passwordEncrypter = passwordEncrypter;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task Execute(RequestResetPasswordCommunication request)
        {
            ValidateRequest(request);

            // Valida o token de recuperação
            if (!_passwordResetTokenService.ValidatePasswordResetToken(request.Token, out ClaimsPrincipal principal))
                throw new ErrorOnValidationException(new List<string> { "Invalid or expired token." });

            // Extrai o identificador do usuário do token (supondo que o token contenha um claim "userId")
            var userId = principal.FindFirst("userId")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                throw new ErrorOnValidationException(new List<string> { "Token does not contain a valid user identifier." });

            //transformar string em int
            var userIdInt = Convert.ToInt32(userId);

            // Busca o usuário pelo id
            var user = await _userRepository.GetUserById(userIdInt);
            if (user == null)
                throw new ErrorOnValidationException(new List<string> { "User not found." });

            // Aplica o hash à nova senha
            var keyAddition = _configuration.GetValue<string>("AdditionalKey:Key");
            var hashedPassword = _passwordEncrypter.Encript(request.Password, keyAddition!);

            // Atualiza a senha do usuário
            user.Password = hashedPassword;
            _userRepository.UpdateUser(user);
            await _unitOfWork.Commit();
        }

        private void ValidateRequest(RequestResetPasswordCommunication request)
        {
            var response = new ResetValidation().Validate(request);
            if (!response.IsValid)
            {
                var errorMessages = response.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
