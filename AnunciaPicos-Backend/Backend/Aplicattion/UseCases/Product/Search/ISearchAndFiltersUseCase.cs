using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Communication.Response.Product;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Search
{
    public interface ISearchAndFiltersUseCase
    {
        public Task<ResponseProductSearchCommunication> ExecuteSearch(RequestProductSearchCommunication request);
        public Task<List<string>> ExecuteAutoComplete(string term);

    }
}
