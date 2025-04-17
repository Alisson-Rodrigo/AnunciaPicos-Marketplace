using AnunciaPicos.Shared.Communication.Request.Gemini;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Gemini
{
    public interface IRequestGeminiChatUseCase
    {
        public Task<string> Execute(RequestChatGeminiCommunication request);

    }
}
