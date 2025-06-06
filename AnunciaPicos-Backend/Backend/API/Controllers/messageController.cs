using AnunciaPicos.Backend.Aplicattion.Services.Chat;
using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Messages;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Shared.Communication.Request.Messages;
using AnunciaPicos.Shared.Communication.Response.Messages;
using AnunciaPicos.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("message")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessagesRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ILogged _logged;

    public MessageController(IMessagesRepository messageRepository, IUserRepository userRepository,
                             IConversationRepository conversationRepository, IHubContext<ChatHub> chatHubContext,
                             IUnitOfWork unitOfWork, ILogged logged)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
        _chatHubContext = chatHubContext;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _logged = logged;
    }

    [Authorize]
    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendMessage([FromBody] RequestSendMessageCommunication messageDTO)
    {
        try
        {
            // Validação de dados de entrada
            if (messageDTO == null || string.IsNullOrEmpty(messageDTO.Message))
                return BadRequest(ResourceMessagesException.MESSAGE_EMPTY);

            UserModel user = await _logged.UserLogged();

            // Verificar se os usuários existem
            var user1 = await _userRepository.GetUserById(user.Id);
            var user2 = await _userRepository.GetUserById(messageDTO.ReceiverId);

            if (user1 == null || user2 == null)
                return BadRequest(ResourceMessagesException.USER_NOT_FOUND);

            if (user1.Id != user.Id)
                return Unauthorized(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

            // Verificar se a conversa já existe
            var conversation = await _conversationRepository.GetConversationByUsers(user1.Id, messageDTO.ReceiverId);

            // Se a conversa não existir, criar uma nova
            if (conversation == null)
            {
                conversation = new ConversationModel
                {
                    UserId1 = user1.Id,
                    UserId2 = messageDTO.ReceiverId,
                    ConversationId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now
                };

                await _conversationRepository.AddConversation(conversation);
                await _unitOfWork.Commit();
            }

            // Criar a nova mensagem
            var newMessage = new MessageModel
            {
                ConversationId = conversation.Id,
                SenderId = user1.Id,
                Message = messageDTO.Message,
                IsRead = false,
                Created = DateTime.Now
            };

            // Salvar a mensagem no banco de dados
            await _messageRepository.AddMessage(newMessage);
            await _unitOfWork.Commit();

            // Criar resposta da mensagem
            var messageResponse = new ResponseSendMessageCommunication
            {
                Id = newMessage.Id,
                ConversationId = newMessage.ConversationId,
                SenderId = newMessage.SenderId,
                SenderName = user1.Name,
                Message = newMessage.Message,
                Created = newMessage.Created,
                IsRead = newMessage.IsRead
            };

            // Enviar mensagem via SignalR para todos os participantes da conversa
            await _chatHubContext.Clients.Group(conversation.ConversationId!)
                .SendAsync("ReceiveMessage", messageResponse);

            // Também enviar notificação específica para o destinatário
            await _chatHubContext.Clients.Group($"user_{messageDTO.ReceiverId}")
                .SendAsync("NewMessageNotification", messageResponse);

            return Ok(messageResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao enviar mensagem: " + ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route("history/{conversationId}")]
    public async Task<IActionResult> GetConversationHistory(string conversationId)
    {
        try
        {
            var user = await _logged.UserLogged();

            // Verifica se a conversa existe
            var conversation = await _conversationRepository.GetConversationByConversationId(conversationId);

            if (conversation == null)
                return NotFound(ResourceMessagesException.CONVERSATION_NOT_EXISTS);

            // Verificar se o usuário tem permissão para acessar esta conversa
            if (conversation.UserId1 != user.Id && conversation.UserId2 != user.Id)
                return Unauthorized(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

            // Recupera as mensagens associadas à conversa
            var messages = await _messageRepository.GetMessagesByConversationId(conversationId);

            if (messages == null || messages.Count == 0)
                return Ok(new List<ResponseSendMessageCommunication>());

            // Mapear as mensagens para DTOs para a resposta
            var messageDtos = messages.Select(m => new ResponseSendMessageCommunication
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                SenderName = m.Sender?.Name ?? "Nome não encontrado",
                Message = m.Message,
                Created = m.Created,
                IsRead = m.IsRead
            }).ToList();

            return Ok(messageDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro ao carregar as mensagens: " + ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    [Route("conversations")]
    public async Task<IActionResult> GetUserConversations()
    {
        try
        {
            var user = await _logged.UserLogged();

            // Implementar método para buscar conversas do usuário
            // var conversations = await _conversationRepository.GetConversationsByUserId(user.Id);

            // Retornar lista de conversas do usuário
            return Ok(/* conversations */);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro ao carregar conversas: " + ex.Message);
        }
    }
}