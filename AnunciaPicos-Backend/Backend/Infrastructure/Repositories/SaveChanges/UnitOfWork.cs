using AnunciaPicos.Backend.Infrastructure.Data;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnunciaPicosDbContext _context;

        public UnitOfWork(AnunciaPicosDbContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
