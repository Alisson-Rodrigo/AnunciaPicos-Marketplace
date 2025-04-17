using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Exceptions.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Aplicattion.Services.Stripe;
using AnunciaPicos.Backend.Infrastructure.Repositories.Payment;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.PlanPayment.PostPlan
{
    public class PlanPaymentUseCase : IPlanPaymentUseCase
    {
        private readonly ILogged _logged;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStripeService _stripeService;
        private readonly IPaymentRepository _paymentRepository;

        public PlanPaymentUseCase(
            ILogged logged,
            IPaymentRepository subscriptionRepository,
            IStripeService stripeService,
            IUnitOfWork unitOfWork,
            IPaymentRepository paymentRepository)
        {
            _logged = logged;
            _paymentRepository = subscriptionRepository;
            _stripeService = stripeService;
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseStripe> Execute(int planId)
        {
            // Validação do tipo de plano
            if ((PlanTypeEnum)planId != PlanTypeEnum.VIP && (PlanTypeEnum)planId != PlanTypeEnum.Master)
            {
                throw new AnunciaPicosExceptions("Plano inválido");
            }

            var user = await _logged.UserLogged();
            var planType = (PlanTypeEnum)planId;

            // Verifica se já tem um plano ativo
            var activePlan = await _paymentRepository.GetActivePlan(user.Id);

            // Se não tem plano ativo, verifica se tem plano pendente

            if (activePlan != null)
            {
                throw new ErrorOnValidationException(new List<string> { "Você já tem um plano ativo" });
            }

            var pendingSubscription = await _paymentRepository.GetAllPlanUser(user.Id);
            if (pendingSubscription != null)
            {
                var timeElapsed = DateTime.Now - pendingSubscription.PurchaseDate;
                if (timeElapsed.TotalMinutes > 10)
                {
                    _paymentRepository.RemovePayment(pendingSubscription);
                    await _unitOfWork.Commit();
                }
                else
                {
                    throw new ErrorOnValidationException(new List<string> { "Há uma solicitação de plano pendente. Por favor, aguarde." });
                }
            }

            // Cria o pagamento no Stripe para novo plano
            var stripeResponse = await _stripeService.CreateOneTimePayment(user, planType);

            // Registra o pagamento no sistema
            var paymentRecord = new PaymentModel
            {
                UserId = user.Id,
                PlanType = planType,
                SessionId = stripeResponse.SessionId,
                PaymentIntentId = stripeResponse.PaymentIntentId,
                StripeInvoiceUrl = stripeResponse.PaymentUrl,
                Amount = GetPlanPrice(planType),
                PurchaseDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMonths(1),
                Status = PaymentStatus.Pending
            };

            await _paymentRepository.AddAsync(paymentRecord);
            await _unitOfWork.Commit();

            return stripeResponse;
        }

        private decimal GetPlanPrice(PlanTypeEnum planType)
        {
            // Implementação da lógica para retornar o preço de cada plano
            switch (planType)
            {
                case PlanTypeEnum.VIP:
                    return 99.90m;
                case PlanTypeEnum.Master:
                    return 199.90m;
                default:
                    throw new ArgumentException("Tipo de plano inválido");
            }
        }
    }
}