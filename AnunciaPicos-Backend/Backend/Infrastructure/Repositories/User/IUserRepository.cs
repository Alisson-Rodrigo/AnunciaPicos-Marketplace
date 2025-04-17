using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.User
{
    public interface IUserRepository
    {
        public Task<bool> GetUserByUsername(string username);
        public Task<UserModel> GetUserById(int id);
        public Task RegisterUser(UserModel user);
        public Task<UserModel> VerifyEmailAndPassword(string email, string password);
        public Task<bool> VerifyCpfExists(string cpf);
        public Task<bool> VerifyEmailExists(string email);
        public Task<UserModel> GetByUserEmail(string email);
        public void UpdateUser(UserModel user);
        public void DeleteUser(UserModel user);





    }
}
