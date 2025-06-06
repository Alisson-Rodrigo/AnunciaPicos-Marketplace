using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public interface IConversationRepository
    {
        Task<ConversationModel> GetConversationByUsers(int userId1, int userId2);
        Task<ConversationModel> GetConversationByConversationId(string conversationId);
        Task<List<ConversationModel>> GetConversationsByUserId(int userId);
        Task AddConversation(ConversationModel conversation);
        Task UpdateConversation(ConversationModel conversation);
        Task DeleteConversation(int conversationId);
    }
}
