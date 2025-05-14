using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Shared.Communication.Request.Product
{
    public class RequestProductSearchCommunication
    {
        public string? OrderBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool Ascending { get; set; } = true;

        public string? Name { get; set; }
        public CategoriesProduct? CategoryId { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }

        public int? UserId { get; set; }

    }

}
