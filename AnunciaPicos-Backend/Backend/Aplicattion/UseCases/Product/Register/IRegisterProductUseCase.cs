using AnunciaPicos.Shared.Communication.Request.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Register
{
    public interface IRegisterProductUseCase
    {
        public Task Execute(RequestRegisterProductCommunication request);
    }
}
