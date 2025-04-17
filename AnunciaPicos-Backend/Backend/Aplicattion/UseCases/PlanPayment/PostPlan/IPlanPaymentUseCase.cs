namespace AnunciaPicos.Backend.Aplicattion.UseCases.PlanPayment.PostPlan
{
    public interface IPlanPaymentUseCase
    {
        public Task<ResponseStripe> Execute(int planId);

    }
}