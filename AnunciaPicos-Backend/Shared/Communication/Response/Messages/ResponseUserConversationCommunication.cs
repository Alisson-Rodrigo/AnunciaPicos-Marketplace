namespace AnunciaPicos.Shared.Communication.Response.Messages
{
    public class ResponseUserConversationCommunication
    {
        public string ConversationId { get; set; } = string.Empty;
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; } = string.Empty;
        public string? OtherUserProfilePicture { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageDate { get; set; }
        public int UnreadMessagesCount { get; set; }
        public bool IsLastMessageFromMe { get; set; }
    }
}

