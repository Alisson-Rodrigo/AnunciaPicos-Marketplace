using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Repositories.Messages;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AnunciaPicos.Backend.Aplicattion.Services.Chat
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly ILogged _logged;

        public ChatHub(IUserRepository userRepository,
                      IConversationRepository conversationRepository,
                      ILogged logged)
        {
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
            _logged = logged;
        }

        // Conectar usuário ao hub
        public override async Task OnConnectedAsync()
        {
            try
            {
                var user = await _logged.UserLogged();
                if (user != null)
                {
                    // Adicionar usuário ao grupo com seu ID
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{user.Id}");

                    // Notificar outros usuários que este usuário está online
                    await Clients.Others.SendAsync("UserOnline", user.Id, user.Name);
                }
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao conectar usuário: {ex.Message}");
            }

            await base.OnConnectedAsync();
        }

        // Desconectar usuário do hub
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = await _logged.UserLogged();
                if (user != null)
                {
                    // Remover usuário do grupo
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{user.Id}");

                    // Notificar outros usuários que este usuário está offline
                    await Clients.Others.SendAsync("UserOffline", user.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desconectar usuário: {ex.Message}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Entrar em uma conversa específica
        // No seu ficheiro ChatHub.cs


public async Task JoinConversation(string conversationId)
    {
        try
        {
            // Obtenha o ID do usuário a partir da claim "Sid"
            var userIdString = Context.User.FindFirst(ClaimTypes.Sid)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                await Clients.Caller.SendAsync("Error", "Utilizador não autenticado.");
                return;
            }

            var conversation = await _conversationRepository.GetConversationByConversationId(conversationId);
            if (conversation != null && (conversation.UserId1 == userId || conversation.UserId2 == userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
                await Clients.Caller.SendAsync("JoinedConversation", conversationId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Não autorizado a entrar nesta conversa");
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", $"Erro ao entrar na conversa: {ex.Message}");
        }
    }


    // Sair de uma conversa específica
    public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
            await Clients.Caller.SendAsync("LeftConversation", conversationId);
        }

        // Marcar mensagem como lida
        public async Task MarkMessageAsRead(int messageId)
        {
            try
            {
                var user = await _logged.UserLogged();
                if (user != null)
                {
                    // Aqui você pode implementar a lógica para marcar a mensagem como lida
                    // e notificar o remetente
                    await Clients.Others.SendAsync("MessageRead", messageId, user.Id);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Erro ao marcar mensagem como lida: {ex.Message}");
            }
        }

        // Notificar que o usuário está digitando
        public async Task UserTyping(string conversationId, bool isTyping)
        {
            try
            {
                var user = await _logged.UserLogged();
                if (user != null)
                {
                    await Clients.GroupExcept(conversationId, Context.ConnectionId)
                        .SendAsync("UserTyping", user.Id, user.Name, isTyping);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Erro ao notificar digitação: {ex.Message}");
            }
        }
    }
}