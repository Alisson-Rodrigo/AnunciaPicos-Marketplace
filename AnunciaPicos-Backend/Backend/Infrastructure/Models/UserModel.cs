using AnunciaPicos.Backend.Infrastructure.Models;
using System.Collections.Generic;

public class UserModel : InfoBaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? ImageProfile {  get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    public List<ProductModel>? Products { get; set; }
    public ICollection<ConversationModel>? Conversation { get; set; }

    public virtual ICollection<PaymentModel>? Payments { get; set; }
    public ICollection<FavoriteModel>? Favorites { get; set; }

    public PaymentModel? ActivePayment => Payments?
        .Where(p => p.Status == PaymentStatus.Completed &&
                   p.ExpirationDate > DateTime.UtcNow)
        .OrderByDescending(p => p.ExpirationDate)
        .FirstOrDefault();
}