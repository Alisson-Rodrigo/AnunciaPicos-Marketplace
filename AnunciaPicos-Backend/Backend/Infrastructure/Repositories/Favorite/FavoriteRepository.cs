using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Favorite
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AnunciaPicosDbContext _context;
        public FavoriteRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }
        public async Task Add(FavoriteModel favorite)
        {
            await _context.Favorites.AddAsync(favorite);
        }

        public void Remove(FavoriteModel favorite)
        {
            _context.Favorites.Remove(favorite);
        }

        public async Task<List<FavoriteModel>> GetFavorites(int id)
        {
            return await _context.Favorites
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ToListAsync();
        }

        public async Task<FavoriteModel?> GetFavoriteByUserIdAndProductId(int userId, int productId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }
}
