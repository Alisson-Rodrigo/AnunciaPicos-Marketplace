using AnunciaPicos.Backend.Aplicattion.Services.HashPassword;
using AnunciaPicos.Backend.Aplicattion.Services.TokenLogin;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;


namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly TokenJwt _tokenGenerator;
        private readonly IPasswordEncrypter _passwordEncrypter;


        public LoginUseCase(IPasswordEncrypter passwordEncrypter, IConfiguration configuration, IUserRepository userRepository, TokenJwt tokenJwt)
        {
            _passwordEncrypter = passwordEncrypter;
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenGenerator = tokenJwt;
        }

        public async Task<ResponseLoginCommunication> Execute(RequestLoginCommunication request)
        {
            ValidateUser(request);

            var keyAddition = _configuration.GetValue<string>("AdditionalKey:Key");
            request.Password = _passwordEncrypter.Encript(request.Password, keyAddition!);

            ResponseLoginCommunication verifyCredentials = await VerifyLoginCredential(request);

            return verifyCredentials;


        }

        public void ValidateUser(RequestLoginCommunication request)
        {
            var validator = new LoginValidation();
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }

        public async Task<ResponseLoginCommunication> VerifyLoginCredential(RequestLoginCommunication request)
        {
            var user = await _userRepository.VerifyEmailAndPassword(request.Email, request.Password);

            if (user == null)
            {
                throw new LoginInvalidException();
            }

            return new ResponseLoginCommunication
            {
                Name = user.Name,
                Token = _tokenGenerator.GenerateToken(user)
            };
        }
    }
}
