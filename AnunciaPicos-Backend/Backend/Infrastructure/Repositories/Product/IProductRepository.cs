using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Product;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Product
{
    public interface IProductRepository
    {
        public Task Register (ProductModel request);
        public Task<List<ProductModel>> GetProducts();
        public Task<ProductModel> GetProductId(int id);
        public void UpdateProduct(ProductModel request);
        public void DeleteProduct(ProductModel product);
        public Task<List<ProductModel>> SearchProductsAsync(RequestProductSearchCommunication request);
        public Task<List<string>> GetProductSuggestionsAsync(string term, int maxResults = 5);
    }
}
