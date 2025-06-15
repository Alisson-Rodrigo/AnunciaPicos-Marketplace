// using ZstdSharp.Unsafe; // 4. REMOVIDO: Using desnecessário
using AnunciaPicos.Shared.Communication.Request.Gemini;
using Microsoft.Extensions.Configuration; // Adicionado para IConfiguration, se já não estiver global
using System.Text.Json;
using System.Text;
using AnunciaPicos.Exceptions.ExceptionBase;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Gemini
{
    public class RequestGeminiChatUseCase : IRequestGeminiChatUseCase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        // 3. REMOVIDO: O campo _configuration é desnecessário aqui
        // private readonly IConfiguration _configuration;

        // O construtor recebe 'configuration' como parâmetro
        public RequestGeminiChatUseCase(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();

            // 1. CORREÇÃO: Usar o parâmetro 'configuration' (com 'c' minúsculo) diretamente
            // para ler a chave da API.
            _apiKey = configuration["Gemini:ApiKey"] ??
                      throw new AnunciaPicosExceptions("API Key do Gemini não foi configurada.");

            // 2. REMOVIDO: Esta linha não é mais necessária, pois não estamos armazenando 'configuration'
            // _configuration = configuration;
        }

        public async Task<string> Execute(RequestChatGeminiCommunication request)
        {
            // O resto do seu código está perfeito e não precisa de alterações.
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var body = new
            {
                contents = request.Contents.Select(c => new
                {
                    role = c.Role,
                    parts = new[] { new { text = c.Text } }
                })
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                // Dica: Logar o 'errorResponse' pode ajudar a depurar erros da API
                throw new AnunciaPicosExceptions($"Erro na comunicação com a API do Gemini. Status: {response.StatusCode}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(resultJson);

            // Adicionando um pouco mais de segurança para evitar NullReferenceException aqui também
            if (!document.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                throw new AnunciaPicosExceptions("A resposta da API do Gemini não contém 'candidates'.");
            }

            var resposta = candidates[0]
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