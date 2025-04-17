using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Migrations;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public interface IMessagesRepository
    {
        public Task AddMessage(MessageModel message);

        public Task<List<MessageModel>> GetMessagesByConversationId(string conversationId);

}
}
