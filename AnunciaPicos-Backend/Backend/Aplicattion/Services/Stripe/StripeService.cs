using Stripe.Checkout;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnunciaPicos.Backend.Aplicattion.Services.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly string _priceMaster;
        private readonly string _priceVip;

        public StripeService()
        {
            // Inicializa a chave de API do Stripe
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY");
            if (string.IsNullOrEmpty(StripeConfiguration.ApiKey))
            {
                throw new InvalidOperationException("Chave da API do Stripe não configurada. Defina a variável de ambiente STRIPE_API_KEY.");
            }
            _priceMaster = Environment.GetEnvironmentVariable("STRIPE_PRICE_MASTER");
            _priceVip = Environment.GetEnvironmentVariable("STRIPE_PRICE_VIP");
            // Validação rigorosa de todas as variáveis
            if (string.IsNullOrEmpty(StripeConfiguration.ApiKey))
                throw new InvalidOperationException("Chave da API do Stripe não configurada (STRIPE_API_KEY).");

            if (string.IsNullOrEmpty(_priceMaster) || string.IsNullOrEmpty(_priceVip))
                throw new InvalidOperationException("IDs de preço não configurados (STRIPE_PRICE_MASTER/VIP).");
        }

        public async Task<ResponseStripe> CreateOneTimePayment(UserModel user, PlanTypeEnum planType)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = GetOneTimePriceId(planType),
                        Quantity = 1,
                    }
                },
                Mode = "payment", // Modo de pagamento único
                SuccessUrl = "https://seusite.com/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://seusite.com/cancel",
                CustomerEmail = user.Email,
                Metadata = new Dictionary<string, string>
                {
                    { "user_id", user.Id.ToString() },
                    { "plan_type", planType.ToString() }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return new ResponseStripe(
                session.Id,           // SessionId
                session.Url,          // PaymentUrl
                session.PaymentIntentId ?? string.Empty, // Fornece valor padrão
                session.ExpiresAt
            );
        }

        private string GetOneTimePriceId(PlanTypeEnum planType)
        {
            // IDs de preços para pagamentos únicos (configure no painel do Stripe)
            switch (planType)
            {
                case PlanTypeEnum.VIP:
                    return _priceVip; // Substitua pelo ID correto
                case PlanTypeEnum.Master:
                    return _priceMaster; // Substitua pelo ID correto
                default:
                    throw new ArgumentException("Invalid plan type");
            }
        }
    }
}