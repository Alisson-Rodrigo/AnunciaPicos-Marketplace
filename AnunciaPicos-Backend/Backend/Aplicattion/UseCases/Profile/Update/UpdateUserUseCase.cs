using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
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
            await ValidateUser(request, user);

            var imagensUrl = string.Empty;

            if (request.ImageProfile != null)
            {
                var nomeArquivo = $"{Guid.NewGuid()}_{request.ImageProfile.FileName}";
                var caminho = Path.Combine("wwwroot/profile/images", nomeArquivo);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    await request.ImageProfile.CopyToAsync(stream);
                }

                var url = $"https://api.anunciapicos.shop/profile/images/{nomeArquivo}";
                imagensUrl = url;
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.UpdatedAt = DateTime.Now;
            if (!string.IsNullOrEmpty(imagensUrl))
            {
                user.ImageProfile = imagensUrl;
            }

            _userRepository.UpdateUser(user);

            await _unitOfWork.Commit();
        }

        public async Task ValidateUser(RequestUpdateProfileCommunication request, UserModel user)
        {
            var validator = new UpdateUserValidation();
            var result = validator.Validate(request);

            if (!request.Email.Equals(user.Email))
            {
                if (await _userRepository.VerifyEmailExists(request.Email))
                {
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_EXISTS));
                }
            }

            if (!request.Name.Equals(user.Name))
            {
                if (await _userRepository.GetUserByUsername(request.Name))
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
