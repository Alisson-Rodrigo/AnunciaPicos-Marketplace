using AnunciaPicos.Shared.Communication.Request.Gemini;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using AnunciaPicos.Exceptions.ExceptionBase;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Gemini
{
    public class RequestGeminiChatUseCase : IRequestGeminiChatUseCase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyCnbEL0mX8YnJePE_7Lm05qJgR5jTDu6B0";

        public RequestGeminiChatUseCase(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> Execute(RequestChatGeminiCommunication request)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var body = new
            {
                contents = request.Contents.Select(c => new
                {
                    role = c.Role,
                    parts = new[]
                    {
                    new { text = c.Text }
                }
                })
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new AnunciaPicosExceptions("Erro na API do Gemini");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(resultJson);

            var resposta = document.RootElement
                                   .GetProperty("candidates")[0]
                                   .GetProperty("content")
                                   .GetProperty("parts")[0]
                                   .GetProperty("text")
                                   .GetString();

            if (string.IsNullOrEmpty(resposta))
            {
                throw new AnunciaPicosExceptions("Resposta vazia da API do Gemini");
            }

            return resposta;
        }
    }
}
