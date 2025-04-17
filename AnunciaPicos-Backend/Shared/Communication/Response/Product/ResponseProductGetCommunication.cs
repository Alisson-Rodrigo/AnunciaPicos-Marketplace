using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Shared.Communication.Response.Product
{
    public class ResponseProductGetCommunication
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CategoriesProduct Categories { get; set; }
        public ICollection<string>? ImageUrl { get; set; } // <- aqui!

    }
}
