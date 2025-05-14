using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Favorite
{
    public interface IFavoriteRepository
    {
        public Task Add(FavoriteModel favorite);
        public void Remove(FavoriteModel favorite);
        public Task<List<FavoriteModel>> GetFavorites();
        public Task<FavoriteModel?> GetFavoriteByUserIdAndProductId(int userId, int productId);


    }
}
