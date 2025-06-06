using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AnunciaPicos.Backend.Infrastructure.Repositories.Messages
{
    // Esta classe implementa ambas as interfaces. Para projetos maiores,
    // considere ter uma classe para cada interface para manter as responsabilidades separadas.
    public class MessagesRepository : IMessagesRepository, IConversationRepository
    {
        private readonly AnunciaPicosDbContext _context;

        public MessagesRepository(AnunciaPicosDbContext context)
        {
            _context = context;
        }

        #region Messages
        public async Task AddMessage(MessageModel message)
        {
            await _context.Message.AddAsync(message);
        }

        public async Task<List<MessageModel>> GetMessagesByConversationId(string conversationId)
        {
            // Busca as mensagens da conversa, incluindo o remetente, e ordena pela data de criação.
            var messages = await _context.Message
                .Where(m => m.Conversation.ConversationId == conversationId)
                .Include(m => m.Sender)
                .OrderBy(m => m.Created)
                .ToListAsync();

            return messages;
        }

        public async Task<MessageModel> GetMessageById(int messageId)
        {
            // Busca uma mensagem específica pelo seu ID, incluindo dados do remetente e da conversa.
            return await _context.Message
                .Include(m => m.Sender)
                .Include(m => m.Conversation)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public Task UpdateMessage(MessageModel message)
        {
            // Marca a entidade da mensagem como modificada. A chamada a SaveChanges/Commit fará a atualização.
            _context.Message.Update(message);
            return Task.CompletedTask;
        }

        public async Task DeleteMessage(int messageId)
        {
            var message = await GetMessageById(messageId);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
        }

        public async Task MarkMessagesAsRead(string conversationId, int userId)
        {
            // Encontra todas as mensagens na conversa que não foram enviadas pelo usuário atual e que ainda não foram lidas.
            var messagesToMarkAsRead = await _context.Message
                .Where(m => m.Conversation.ConversationId == conversationId &&
                               m.SenderId != userId && !m.IsRead)
                .ToListAsync();

            if (messagesToMarkAsRead.Any())
            {
                // Marca cada uma como lida. O Unit of Work se encarregará de chamar SaveChanges/Commit.
                foreach (var message in messagesToMarkAsRead)
                {
                    message.IsRead = true;
                }
            }
        }

        public async Task<int> GetUnreadMessageCount(int userId)
        {
            // Conta o número total de mensagens não lidas destinadas ao usuário em todas as conversas.
            return await _context.Message
                .CountAsync(m => (m.Conversation.UserId1 == userId || m.Conversation.UserId2 == userId) &&
                                 m.SenderId != userId &&
                                 !m.IsRead);
        }
        #endregion

        #region Conversations
        public async Task AddConversation(ConversationModel conversation)
        {
            await _context.Conversation.AddAsync(conversation);
        }

        public async Task<ConversationModel> GetConversationByUsers(int userId1, int userId2)
        {
            // Busca uma conversa entre dois usuários, não importando quem é UserId1 ou UserId2.
            return await _context.Conversation
                .FirstOrDefaultAsync(c => (c.UserId1 == userId1 && c.UserId2 == userId2) ||
                                            (c.UserId1 == userId2 && c.UserId2 == userId1));
        }

        public async Task<ConversationModel> GetConversationByConversationId(string conversationId)
        {
            // Busca uma conversa pelo seu identificador único.
            return await _context.Conversation
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

        public async Task<List<ConversationModel>> GetConversationsByUserId(int userId)
        {
            // Busca todas as conversas de um usuário, incluindo os dados do outro participante
            // e a última mensagem trocada para exibição em uma lista de conversas.
            return await _context.Conversation
                .Where(c => c.UserId1 == userId || c.UserId2 == userId)
                .Include(c => c.userModel1) // CORRIGIDO: Inclui os dados do usuário 1
                .Include(c => c.userModel2) // CORRIGIDO: Inclui os dados do usuário 2
                .Include(c => c.Messages.OrderByDescending(m => m.Created).Take(1)) // Inclui a última mensagem
                .OrderByDescending(c => c.Messages.Any() ? c.Messages.Max(m => m.Created) : c.CreatedAt) // Ordena pela mais recente
                .ToListAsync();
        }

        public Task UpdateConversation(ConversationModel conversation)
        {
            _context.Conversation.Update(conversation);
            return Task.CompletedTask;
        }

        public async Task DeleteConversation(int conversationId)
        {
            // Encontra a conversa pelo seu ID de banco de dados (PK)
            var conversation = await _context.Conversation.FindAsync(conversationId);
            if (conversation != null)
            {
                _context.Conversation.Remove(conversation);
            }
        }
        #endregion
    }
}
