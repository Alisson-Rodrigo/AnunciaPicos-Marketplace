using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Delete
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogged _logged;

        public DeleteUserUseCase(IUserRepository userRepository, ILogged logged, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logged = logged;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute()
        {
            var user = await _logged.UserLogged();

            _userRepository.DeleteUser(user);

            await _unitOfWork.Commit();
        }
    }
}
