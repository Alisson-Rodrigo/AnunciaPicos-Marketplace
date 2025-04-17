using AnunciaPicos.Shared.Communication.Request.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Update
{
    public interface IUpdateProductUseCase
    {

        public Task Execute(RequestUpdateProductCommunication request, int id);
    }
}
