using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class ProductModel : InfoBaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public float Price { get; set; }

        public CategoriesProduct Category { get; set; }

        // Chave estrangeira para User
        public int? UserId { get; set; } // Relacionamento com UserModel
        public ICollection<string>? ImageUrl { get; set; } // URL da imagem do produto
        public UserModel? User { get; set; }
    }
}
