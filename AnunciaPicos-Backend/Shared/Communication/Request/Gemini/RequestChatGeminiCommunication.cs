namespace AnunciaPicos.Shared.Communication.Request.Gemini
{
    public class RequestChatGeminiCommunication
    {

        public required List<Mensagem> Contents { get; set; }

        public class Mensagem
        {
            public required string Role { get; set; } // "user" ou "model"
            public required string Text { get; set; }
        }
    }
}
