using AnunciaPicos.Shared.Communication.Request.Evaluation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Register
{
    public interface IPostEvaluationUseCase
    {
        public Task Execute(RequestEvaluationCommunication request);

    }
}
