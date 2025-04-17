using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Evaluation
{
    public interface IEvaluationRepository
    {
        public Task RegisterEvaluation(EvaluationModel evaluation);

        public Task<List<EvaluationModel>> GetEvaluationSeller(int id);

        public Task<bool> VerifyUserCommentSeller(int userId, int userIdEvaluated);
        public Task<double> GetAverageNoteById(int id);


    }
}
