using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using AnunciaPicos.Shared.Communication.Request.Gemini;
using AnunciaPicos.Backend.Aplicattion.UseCases.Gemini;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Authorization;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("gemini")]
    [ApiController]
    public class integrationGeminiController : ControllerBase
    {

        [Authorize]
        [HttpPost("gemini/chat")]
        public async Task<IActionResult> ConversaGemini([FromBody] RequestChatGeminiCommunication request, IRequestGeminiChatUseCase requestGeminiChatUseCase)
        {
            var response = await requestGeminiChatUseCase.Execute(request);
            return Ok(new { response });
        }
    }

}