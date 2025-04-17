using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Evaluation
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public EvaluationRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        public async Task RegisterEvaluation (EvaluationModel evaluation)
        {
            await _context.Evaluation.AddAsync(evaluation);
        }

        public async Task<List<EvaluationModel>> GetEvaluationSeller(int id)
        {
            return await _context.Evaluation
                .Where(e => e.UserIdEvaluated == id)
                .ToListAsync();
        }

        public async Task<bool> VerifyUserCommentSeller(int userId, int userIdEvaluated)
        {
            // Verifica se já existe uma avaliação feita pelo usuário sobre o vendedor
            var exists = await _context.Evaluation
                .AnyAsync(e => e.UserId == userId && e.UserIdEvaluated == userIdEvaluated);

            return exists;
        }

        public async Task<double> GetAverageNoteById(int id)
        {
            var media = await _context.Evaluation
                .Where(e => e.UserIdEvaluated == id)
                .Select(e => (double?)e.Note)
                .AverageAsync();

            return media.GetValueOrDefault(); // Se media for null, retorna 0
        }






    }
}
