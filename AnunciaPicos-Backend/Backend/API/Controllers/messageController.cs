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
public class messageController : ControllerBase
{
    private readonly IMessagesRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly ILogged _logged;

    public messageController(IMessagesRepository messageRepository, IUserRepository userRepository,
                             IConversationRepository conversationRepository, IHubContext<ChatHub> chatHubContext,
                             IUnitOfWork unitOfWork,
                             ILogged logged)
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
        // Validação de dados de entrada
        if (messageDTO == null || string.IsNullOrEmpty(messageDTO.Message))
            return BadRequest(ResourceMessagesException.MESSAGE_EMPTY);

        // Verificar se os usuários existem
        var user1 = await _userRepository.GetUserById(messageDTO.SenderId);
        var user2 = await _userRepository.GetUserById(messageDTO.ReceiverId);

        UserModel user = await _logged.UserLogged();

        if (user1 == null || user2 == null)
            return BadRequest(ResourceMessagesException.USER_NOT_FOUND);

        if (user1.Id != user.Id)
            return Unauthorized(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        // Verificar se a conversa já existe
        var conversation = await _conversationRepository.GetConversationByUsers(messageDTO.SenderId, messageDTO.ReceiverId);

        // Se a conversa não existir, criar uma nova
        if (conversation == null)
        {
            conversation = new ConversationModel
            {
                UserId1 = messageDTO.SenderId,
                UserId2 = messageDTO.ReceiverId,
                ConversationId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            };

            // Salvar a conversa no banco de dados
            await _conversationRepository.AddConversation(conversation);
            await _unitOfWork.Commit();  // Garantir que a conversa seja salva antes de continuar
        }

        // Criar a nova mensagem
        var newMessage = new MessageModel
        {
            ConversationId = conversation.Id,  // Garantir que o ConversationId seja válido
            SenderId = messageDTO.SenderId,
            Message = messageDTO.Message,
            IsRead = false,
            Created = DateTime.Now
        };

        try
        {
            // Salvar a mensagem no banco de dados
            await _messageRepository.AddMessage(newMessage);

            // Enviar a mensagem para todos os participantes da conversa através do SignalR
            await _chatHubContext.Clients.Group(conversation.ConversationId!).SendAsync("ReceiveMessage",
                new ResponseSendMessageCommunication
                {
                    Id = newMessage.Id,
                    ConversationId = newMessage.ConversationId,
                    SenderId = newMessage.SenderId,
                    SenderName = user1.Name, // Nome do remetente
                    Message = newMessage.Message,
                    Created = newMessage.Created,
                    IsRead = newMessage.IsRead
                });

            // Commit nas mudanças no banco
            await _unitOfWork.Commit();

            // Retornar a resposta com a mensagem enviada
            return Ok(new ResponseSendMessageCommunication
            {
                Id = newMessage.Id,
                ConversationId = newMessage.ConversationId,
                SenderId = newMessage.SenderId,
                SenderName = user1.Name,
                Message = newMessage.Message,
                Created = newMessage.Created,
                IsRead = newMessage.IsRead
            });
        }
        catch (Exception ex)
        {
            // Em caso de erro ao salvar a mensagem
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao salvar a mensagem");
        }
    }

    [Authorize]
    [HttpGet]
    [Route("history/{conversationId}")]
    public async Task<IActionResult> GetConversationHistory(string conversationId)
    {
        try
        {
            // Verifica se a conversa existe
            var conversation = await _conversationRepository.GetConversationByConversationId(conversationId);

            if (conversation == null)
            {
                return NotFound(ResourceMessagesException.CONVERSATION_NOT_EXISTS);
            }

            // Recupera as mensagens associadas à conversa
            var messages = await _messageRepository.GetMessagesByConversationId(conversationId);

            if (messages == null || messages.Count == 0)
            {
                return Ok(ResourceMessagesException.MESSAGE_EMPTY);
            }

            // Mapear as mensagens para DTOs para a resposta
            var messageDtos = messages.Select(m => new ResponseSendMessageCommunication
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                SenderName = m.Sender?.Name ?? "Nome não encontrado", // Nome do remetente
                Message = m.Message,
                Created = m.Created,
                IsRead = m.IsRead
            }).ToList();

            return Ok(messageDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro ao carregar as mensagens");
        }
    }




}
