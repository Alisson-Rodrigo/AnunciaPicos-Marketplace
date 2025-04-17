namespace AnunciaPicos.Backend.Infrastructure.Repositories.Payment
{
    public interface IPaymentRepository
    {
        public Task<PaymentModel> GetActivePlan(int userId);
        Task AddAsync(PaymentModel payment);
        public void Update(PaymentModel payment);

        // Métodos adicionais para melhor controle
        Task<PaymentModel> GetBySessionIdAsync(string sessionId);
        Task<PaymentModel> GetByPaymentIntentIdAsync(string paymentIntentId);
        public Task<bool> HasActivePlan(int userId, PlanTypeEnum planType);
        public Task<PaymentModel> GetAllPlanUser(int userId);
        public void RemovePayment(PaymentModel payment);
        public Task<List<PaymentModel>> GetExpiredPaymentsAsync();


    }
}