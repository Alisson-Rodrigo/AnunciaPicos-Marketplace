namespace AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges
{
    public interface IUnitOfWork
    {
        public Task Commit();

    }
}
