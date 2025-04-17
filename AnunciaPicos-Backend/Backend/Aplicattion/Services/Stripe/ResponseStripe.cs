public class ResponseStripe
{
    public string SessionId { get; set; }          // ID da sessão de checkout
    public string PaymentUrl { get; set; }         // URL para redirecionamento ao Stripe
    public string PaymentIntentId { get; set; }    // ID do pagamento (para referência futura)
    public DateTime? ExpirationDate { get; set; }  // Data de expiração do plano (opcional)

    public ResponseStripe(string sessionId, string paymentUrl, string paymentIntentId, DateTime? expirationDate = null)
    {
        SessionId = sessionId;
        PaymentUrl = paymentUrl;
        PaymentIntentId = paymentIntentId;
        ExpirationDate = expirationDate;
    }
}