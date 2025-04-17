using AnunciaPicos.Backend.Infrastructure.Models;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public interface IConversationRepository
    {
        public Task<ConversationModel> GetConversationByUsers(int userId1, int userId2);
        public Task AddConversation(ConversationModel conversation);
        public Task<ConversationModel> GetConversationByConversationId(string conversationId);

    }
}
