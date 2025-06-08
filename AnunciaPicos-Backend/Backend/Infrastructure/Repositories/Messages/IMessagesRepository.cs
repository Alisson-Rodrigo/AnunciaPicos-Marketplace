using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Migrations;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public interface IMessagesRepository
    {
        public Task AddMessage(MessageModel message);

        public Task<List<MessageModel>> GetMessagesByConversationId(string conversationId);

        Task<MessageModel?> GetLastMessageByConversationId(int conversationId);
        Task<int> GetUnreadMessagesCount(int conversationId, int userId);

        Task MarkMessagesAsReadByConversation(int conversationId, int userId);
        Task<int> GetTotalUnreadMessagesCount(int userId);
        Task MarkMessageAsRead(int messageId);

    }
}
