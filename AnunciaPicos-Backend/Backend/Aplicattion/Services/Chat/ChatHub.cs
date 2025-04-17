using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using System.Text.RegularExpressions;

public class ChatHub : Hub
{
    // Envia a mensagem para todos os membros do grupo da conversa
    public async Task SendMessage(string sender, string receiver, string message, string conversationId)
    {
        // Certifique-se de que o usuário está na conversa (grupo)
        await Clients.Group(conversationId).SendAsync("ReceiveMessage", sender, message);
    }

    // Adiciona o cliente ao grupo de uma conversa
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    // Remove o cliente do grupo da conversa
    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }
}
