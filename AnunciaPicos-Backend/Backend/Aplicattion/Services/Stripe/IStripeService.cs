namespace AnunciaPicos.Backend.Aplicattion.Services.Stripe
{
    public interface IStripeService
    {
        public Task<ResponseStripe> CreateOneTimePayment(UserModel user, PlanTypeEnum planType);
    }

}
