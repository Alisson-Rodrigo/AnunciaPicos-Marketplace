using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Aplicattion.Services.LoggedUser
{
    public interface ILogged
    {
        public Task<UserModel> UserLogged();

    }
}
