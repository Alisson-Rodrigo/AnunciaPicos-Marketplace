using AnunciaPicos.Backend.Aplicattion.UseCases.PlanPayment.PostPlan;
using AnunciaPicos.Backend.Infrastructure.Repositories.Payment;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class planPaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _webhookSecret;

        private readonly ILogger<planPaymentsController> _logger;

        public planPaymentsController(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ILogger<planPaymentsController> logger)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

            if (string.IsNullOrEmpty(_webhookSecret))
            {
                throw new Exception(ResourceMessagesException.UNKNOW_ERROR);
            }
        }

        [Authorize]
        [HttpPost("checkout/{planId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePayment([FromRoute] int planId, [FromServices] IPlanPaymentUseCase planPaymentUseCase)
        {
            var response = await planPaymentUseCase.Execute(planId);
            return Ok(response);
        }

        [HttpPost("stripe-webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            _logger.LogInformation($"Evento Stripe recebido: {json}");

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret);

                _logger.LogInformation($"Evento Stripe processado: {stripeEvent.Type}");

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var payment = await _paymentRepository.GetBySessionIdAsync(session!.Id);

                    if (payment != null)
                    {
                        // Atualiza status do pagamento atual
                        payment.Status = PaymentStatus.Completed;
                        payment.PaymentIntentId = session.PaymentIntentId;
                        payment.StripeInvoiceUrl = session.Url;

                        // Atualiza o pagamento atual como concluído
                        _paymentRepository.Update(payment);
                        await _unitOfWork.Commit();
                    }
                    else
                    {
                        _logger.LogWarning($"Pagamento não encontrado para sessão: {session.Id}");
                    }
                }
                else if (stripeEvent.Type == "checkout.session.expired")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var payment = await _paymentRepository.GetBySessionIdAsync(session.Id);

                    if (payment != null)
                    {
                        // Marca o pagamento como expirado
                        payment.Status = PaymentStatus.Expired;
                        _paymentRepository.Update(payment);
                        await _unitOfWork.Commit();
                    }
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var payment = await _paymentRepository.GetByPaymentIntentIdAsync(paymentIntent.Id);

                    if (payment != null)
                    {
                        // Marca o pagamento como falho
                        payment.Status = PaymentStatus.Failed;

                        _paymentRepository.Update(payment);
                        await _unitOfWork.Commit();
                    }
                }
                else
                {
                    _logger.LogInformation($"Evento não tratado: {stripeEvent.Type}");
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Erro no processamento do webhook do Stripe");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao processar webhook");
                return StatusCode(500);
            }
        }
    }
}