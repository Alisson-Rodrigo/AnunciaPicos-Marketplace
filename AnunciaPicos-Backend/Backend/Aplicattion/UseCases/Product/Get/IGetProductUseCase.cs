using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Get
{
    public interface IGetProductUseCase
    {
        public Task<List<ResponseProductGetCommunication>> Execute();

    }
}
