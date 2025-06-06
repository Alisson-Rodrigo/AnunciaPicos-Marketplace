using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Migrations;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public interface IMessagesRepository
    {
        Task AddMessage(MessageModel message);
        Task<List<MessageModel>> GetMessagesByConversationId(string conversationId);
        Task<MessageModel> GetMessageById(int messageId);
        Task UpdateMessage(MessageModel message);
        Task DeleteMessage(int messageId);
        Task MarkMessagesAsRead(string conversationId, int userId);
        Task<int> GetUnreadMessageCount(int userId);
    }
}
