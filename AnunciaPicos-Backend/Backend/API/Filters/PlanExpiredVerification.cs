using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Repositories.Payment;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;

namespace AnunciaPicos.Backend.API.Filters
{
    public class PlanExpiredVerification : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PlanExpiredVerification(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var paymentRepo = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var pagamentosExpirados = await paymentRepo.GetExpiredPaymentsAsync();

                foreach (var pagamento in pagamentosExpirados)
                {
                    pagamento.Status = PaymentStatus.Expired;
                    paymentRepo.Update(pagamento);
                }

                await unitOfWork.Commit();

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}

