using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Shared.Communication.Request.Product
{
    public class RequestProductSearchCommunication
    {
        public string? Name { get; set; }
        public CategoriesProduct? CategoryId { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
    }

}
