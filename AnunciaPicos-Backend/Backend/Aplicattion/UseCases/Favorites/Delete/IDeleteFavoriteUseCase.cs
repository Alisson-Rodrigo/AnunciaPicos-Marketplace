namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Delete
{
    public interface IDeleteFavoriteUseCase
    {
        public Task Execute(int productId);

    }
}
