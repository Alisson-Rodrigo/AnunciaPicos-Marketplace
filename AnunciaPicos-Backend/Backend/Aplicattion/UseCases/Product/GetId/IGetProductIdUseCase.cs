using AnunciaPicos.Shared.Communication.Response.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.GetId
{
    public interface IGetProductIdUseCase
    {
        public Task<ResponseProductGetCommunication> Execute(int id);
    }
}
