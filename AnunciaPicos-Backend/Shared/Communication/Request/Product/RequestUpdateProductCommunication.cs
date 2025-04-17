using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Shared.Communication.Request.Product
{
    public class RequestUpdateProductCommunication
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public CategoriesProduct Categories { get; set; }
    }
}
