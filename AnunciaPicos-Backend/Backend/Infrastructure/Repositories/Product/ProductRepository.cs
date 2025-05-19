using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Product;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public ProductRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        public async Task Register(ProductModel request)
        {
            await _context.Products.AddAsync(request);
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            return await _context.Products
                .Where(x => x.Active)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductModel?> GetProductId(int id)
        {
            return await _context.Products
                .Where(x => x.Active)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateProduct(ProductModel request)
        {
            _context.Products.Update(request);
        }

        public void DeleteProduct(ProductModel product)
        {
            _context.Products.Remove(product);
        }

        public async Task<List<ProductModel>> SearchProductsAsync(RequestProductSearchCommunication request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var loweredName = request.Name.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(loweredName));
            }
            if (request.CategoryId.HasValue)
                query = query.Where(p => p.Category == request.CategoryId);

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.UserId.HasValue)
                query = query.Where(p => p.UserId == request.UserId.Value);


            query = request.OrderBy?.ToLower() switch
            {
                "name" => request.Ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "price" => request.Ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            return await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(RequestProductSearchCommunication request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(p => p.Name.Contains(request.Name));

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.Category == request.CategoryId.Value);

            if (request.UserId.HasValue)
                query = query.Where(p => p.UserId == request.UserId.Value);


            return await query.CountAsync();
        }

        public async Task<List<string>> GetProductSuggestionsAsync(string term, int maxResults = 5)
        {
            return await _context.Products
                .Where(p => p.Name.ToLower().Contains(term.ToLower()))
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .Distinct()
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<List<ProductModel>> GetProductsFavorites(List<FavoriteModel> favorites)
        {
            var productIds = favorites.Select(f => f.ProductId).ToList();

            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }


    }
}
