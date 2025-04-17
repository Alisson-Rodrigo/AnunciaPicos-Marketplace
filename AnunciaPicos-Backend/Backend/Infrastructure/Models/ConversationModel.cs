namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class ConversationModel
    {
        public int Id { get; set; }

        //ref de users da conversa
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }

        public string? ConversationId {  get; set; }

        //receber as entity
        public UserModel? userModel1 { get; set; }
        public UserModel? userModel2 { get; set; }
        
        //lista de messages 
        public ICollection<MessageModel>? Messages { get; set; }

        //date time created conversation
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
