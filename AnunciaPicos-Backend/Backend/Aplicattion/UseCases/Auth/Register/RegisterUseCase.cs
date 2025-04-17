using AnunciaPicos.Backend.Aplicattion.Services.HashPassword;
using AnunciaPicos.Backend.Aplicattion.Services.TokenLogin;
using AnunciaPicos.Backend.Aplicattion.Services.ValidatorCpf;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register
{
    public class RegisterUseCase : IRegisterUseCase
    {

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _registerUserMapping;
        private readonly TokenJwt _tokenGenerator;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly IConfiguration _configuration;


        public RegisterUseCase(IUserRepository userRepository,
            IMapper mapping,
            IUnitOfWork unitOfWork,
            TokenJwt tokenGenerator,
            IPasswordEncrypter passwordEncrypter,
            IConfiguration configuration

            )
        {
            _userRepository = userRepository;
            _registerUserMapping = mapping;
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _passwordEncrypter = passwordEncrypter;
            _configuration = configuration;
        }

        public async Task<ResponseRegisterCommunication> Execute(RequestRegisterCommunication request)
        {
           await ValidateUser(request);

            var user = _registerUserMapping.Map<UserModel>(request);

            //gerando hash da senha
            var keyAddition = _configuration.GetValue<string>("AdditionalKey:Key");
            user.Password = _passwordEncrypter.Encript(request.Password, keyAddition!);

            user.UserIdentifier = Guid.NewGuid();

            await _userRepository.RegisterUser(user);

            await _unitOfWork.Commit();

            return new ResponseRegisterCommunication
            {
                Name = user.Name,
                Token = _tokenGenerator.GenerateToken(user)
            };
        }

        public async Task ValidateUser(RequestRegisterCommunication request)
        {
            CPF cpfValidator = new CPF();
            var validator = new RegisterUserValidation(cpfValidator);
            var result = validator.Validate(request);

            var emailExists = await _userRepository.VerifyEmailExists(request.Email);

            if (emailExists == true)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_EXISTS));
            }

            var cpfExists = await _userRepository.VerifyCpfExists(request.CPF);
            if (cpfExists == true) {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.CPF_EXISTS));
            }

            if (!result.IsValid)
            {
                //Pegar a mensagem de erro e lançar uma exceção
                var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();
                //Lançar uma exceção
                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
