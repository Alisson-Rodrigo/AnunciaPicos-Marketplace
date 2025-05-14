namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class FavoriteModel : InfoBaseModel
    {
        public int UserId { get; set; }
        public UserModel? User { get; set; }

        public int ProductId { get; set; }
        public ProductModel? Product { get; set; }
    }

}
