using AnunciaPicos.Shared.Communication.Response.Evaluation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Get
{
    public interface IGetEvaluationUseCase
    {
        public Task<List<ResponseGetEvaluationCommunicattion>> Execute(int id);

    }
}
