using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public UserRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        public async Task<bool> GetUserByUsername(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Name == username);
        }

        public async Task<UserModel> GetUserById(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id && u.Active)
                .SingleOrDefaultAsync();
        }


        public async Task<UserModel> GetByUserEmail (string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> VerifyCpfExists (string cpf)
        {
            return await _context.Users.AnyAsync(u => u.CPF == cpf);
        }

        public async Task RegisterUser(UserModel user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser (UserModel user)
        {
            _context.Users.Update(user);
        }

        public async Task<UserModel> VerifyEmailAndPassword(string email, string password)
        {
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(user =>
                    user.Email.ToLower() == email.ToLower()
                    && user.Password == password);
        }

        public async Task<bool> VerifyEmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public void DeleteUser(UserModel user)
        {
            _context.Users.Remove(user);
        }

    }
}
