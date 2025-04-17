using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Delete;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.GetId;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Profile;
using AnunciaPicos.Shared.Communication.Response.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Operators;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class profileController : ControllerBase
    {
        [Authorize]
        [HttpGet("perfil")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile([FromServices] IGetUserUseCase getUserUseCase)
        {
            ResponseGetProfileCommunication response = await getUserUseCase.Execute();
            return Ok(response);
        }

        [HttpGet("perfil/{id}")]
        [ProducesResponseType(typeof(ResponseGetProfileIdCommunication), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileId([FromServices] IGetUserIdUseCase getUserByIdUseCase, int id)
        {
            ResponseGetProfileIdCommunication response = await getUserByIdUseCase.Execute(id);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("atualizar-perfil")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase updateUserUseCase, [FromBody] RequestUpdateProfileCommunication request)
        {
            await updateUserUseCase.Execute(request);
            return Ok();

        }

        [Authorize]
        [HttpDelete("deletar-perfil")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProfile([FromServices] IDeleteUserUseCase deleteUserUseCase)
        {
            await deleteUserUseCase.Execute();
            return NoContent();
        }
    }
}
