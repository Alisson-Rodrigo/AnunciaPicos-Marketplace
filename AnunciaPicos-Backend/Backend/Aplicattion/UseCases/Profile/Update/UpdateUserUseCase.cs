using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Profile;
using AnunciaPicos.Shared.Exceptions;
using SixLabors.ImageSharp.Formats.Webp;

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
                var nameFileNotExtension = Path.GetFileNameWithoutExtension(request.ImageProfile.FileName);
                var nameFile = $"{Guid.NewGuid()}_{nameFileNotExtension}.webp";
                var caminhoWebP = Path.Combine("wwwroot/profile/images", nameFile);

                using (var inputStream = request.ImageProfile.OpenReadStream())
                using (var image = await Image.LoadAsync(inputStream))
                {
                    var encoder = new WebpEncoder
                    {
                        Quality = 90 // pode ajustar isso se quiser menos qualidade e mais compressão
                    };

                    await image.SaveAsync(caminhoWebP, encoder);
                }

                var url = $"https://api.anunciapicos.shop/profile/images/{nameFile}";
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
