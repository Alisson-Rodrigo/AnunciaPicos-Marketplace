using AnunciaPicos.Backend.Infrastructure.Models;
using System.Collections.Generic;

public class UserModel : InfoBaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }

    // Relacionamento 1:N com produtos (um usuário pode ter vários produtos)
    public List<ProductModel>? Products { get; set; }

    // Relacionamento com conversas
    public ICollection<ConversationModel>? Conversation { get; set; }

    // NOVO: Relacionamento 1:N com pagamentos (um usuário pode ter vários pagamentos/planos ao longo do tempo)
    public virtual ICollection<PaymentModel>? Payments { get; set; } // Correção: manter como ICollection

    // Propriedade auxiliar para obter o plano ativo atual
    public PaymentModel? ActivePayment => Payments?
        .Where(p => p.Status == PaymentStatus.Completed &&
                   p.ExpirationDate > DateTime.UtcNow)
        .OrderByDescending(p => p.ExpirationDate)
        .FirstOrDefault();
}