namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        //relacionamento com tabela de conversas
        public int ConversationId { get; set; }
        public ConversationModel? Conversation { get; set; }

        //user que enviou a mensagem
        public int SenderId { get; set; }
        public UserModel? Sender { get; set; }

        //conteudo da mensagem
        public string Message { get; set; } = string.Empty;

        //data sender
        public DateTime Created { get; set; } = DateTime.Now;

        //Status reader
        public bool IsRead { get; set; }

    }
}
