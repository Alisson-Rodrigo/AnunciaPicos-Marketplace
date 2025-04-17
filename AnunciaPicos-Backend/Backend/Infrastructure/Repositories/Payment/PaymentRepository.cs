using AnunciaPicos.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace AnunciaPicos.Backend.Infrastructure.Repositories.Payment
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public PaymentRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentModel> GetActivePlan(int userId)
        {
            // Retorna o plano mais recente que ainda não expirou
            return await _context.Payments
                .Where(p => p.UserId == userId &&
                           p.Status == PaymentStatus.Completed &&
                           p.ExpirationDate > DateTime.UtcNow)
                .OrderByDescending(p => p.PurchaseDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(PaymentModel payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public void Update(PaymentModel payment)
        {
            _context.Payments.Update(payment);
        }

        // Métodos adicionais úteis
        public async Task<PaymentModel> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.SessionId == sessionId);
        }

        public async Task<PaymentModel> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task<bool> HasActivePlan(int userId, PlanTypeEnum planType)
        {
            return await _context.Payments
                .AnyAsync(p => p.UserId == userId &&
                             p.PlanType == planType &&
                             p.Status == PaymentStatus.Completed &&
                             p.ExpirationDate > DateTime.UtcNow);
        }


        public async Task<PaymentModel> GetAllPlanUser (int userId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public void RemovePayment(PaymentModel payment)
        {
            _context?.Payments.Remove(payment);
        }

        public async Task<List<PaymentModel>> GetExpiredPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Completed && p.ExpirationDate <= DateTime.UtcNow)
                .ToListAsync();
        }


    }
}