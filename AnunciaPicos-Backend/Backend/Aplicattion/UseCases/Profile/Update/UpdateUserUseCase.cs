using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Profile;
using AnunciaPicos.Shared.Exceptions;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILogged _logged;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(ILogged logged, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _logged = logged;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(RequestUpdateProfileCommunication request)
        {
            var user = await _logged.UserLogged();

            ValidateUser(request, user);

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.UpdatedAt = DateTime.Now;

            _userRepository.UpdateUser(user);

            await _unitOfWork.Commit();
        }

        public void ValidateUser(RequestUpdateProfileCommunication request, UserModel user)
        {
            var validator = new UpdateUserValidation();
            var result = validator.Validate(request);

            if (request.Email.Equals(user.Email) == false) {
                if (_userRepository.VerifyEmailExists(request.Email).Result)
                {
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_EXISTS));
                }
            }

            if (request.Name.Equals(user.Name) == false)
            {
                if (_userRepository.GetUserByUsername(request.Name).Result)
                {
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("username", ResourceMessagesException.NAME_EXISTS));
                }
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
