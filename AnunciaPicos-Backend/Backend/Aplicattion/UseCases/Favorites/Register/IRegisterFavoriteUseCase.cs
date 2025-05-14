namespace AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Register
{
    public interface IRegisterFavoriteUseCase
    {
        public Task Execute(int productId);

    }
}
