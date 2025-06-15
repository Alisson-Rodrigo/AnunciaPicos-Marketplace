namespace AnunciaPicos.Shared.Communication.Request.Messages
{
    public class RequestSendMessageCommunication
    {
        public int ReceiverId { get; set; } // ID do usuário que recebe a mensagem
        public string Message { get; set; } // Conteúdo da mensagem
    }

}
