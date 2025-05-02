using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Communication.Response.Product;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mysqlx.Prepare;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Search
{
    public class SearchAndFiltersUseCase : ISearchAndFiltersUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public SearchAndFiltersUseCase(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }

        public async Task<ResponseProductSearchCommunication> ExecuteSearch(RequestProductSearchCommunication request)
        {
            var items = await _productRepository.SearchProductsAsync(request);
            if (items == null || !items.Any())
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }

            var totalItems = await _productRepository.GetTotalCountAsync(request);

            // Nesse ponto, `products` já vai conter a lista de imagens de cada produto
            return new ResponseProductSearchCommunication
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                Items = items
            };
        }

        public async Task<List<string>> ExecuteAutoComplete(string term)
        {
            var suggestions = await _productRepository.GetProductSuggestionsAsync(term);

            if (suggestions == null || !suggestions.Any())
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.NOT_FOUND_PRODUCTS);
            }

            return suggestions;
        }


    }
}
