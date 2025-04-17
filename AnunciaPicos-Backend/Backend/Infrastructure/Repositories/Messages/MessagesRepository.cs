using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    public class MessagesRepository : IMessagesRepository, IConversationRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public MessagesRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        public async Task AddMessage(MessageModel message)
        {
            await _context.Message.AddAsync(message);
        }

        // Obter conversa entre dois usuários
        public async Task<ConversationModel> GetConversationByUsers(int userId1, int userId2)
        {
            return await _context.Conversation
                .FirstOrDefaultAsync(c => (c.UserId1 == userId1 && c.UserId2 == userId2) ||
                                          (c.UserId1 == userId2 && c.UserId2 == userId1));
        }

        // Adicionar nova conversa
        public async Task AddConversation(ConversationModel conversation)
        {
            await _context.Conversation.AddAsync(conversation);
        }

        public async Task<List<MessageModel>> GetMessagesByConversationId(string conversationId)
        {
            try
            {
                var messages = await _context.Message
                    .Where(m => m.Conversation.ConversationId == conversationId) // Filtra pela ConversationId
                    .Include(m => m.Sender) // Inclui os detalhes do usuário que enviou a mensagem
                    .OrderBy(m => m.Created) // Ordena as mensagens pela data de envio
                    .ToListAsync(); // Retorna as mensagens de forma assíncrona

                return messages;
            }
            catch (Exception ex)
            {
                // Log de erro ou manipulação adicional de exceção
                throw new Exception("Erro ao recuperar as mensagens da conversa: " + ex.Message);
            }
        }

        // ConversationRepository.cs
        public async Task<ConversationModel> GetConversationByConversationId(string conversationId)
        {
            return await _context.Conversation
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

    }
}
