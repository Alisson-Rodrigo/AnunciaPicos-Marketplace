using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Delete;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.GetId;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update;
using AnunciaPicos.Shared.Communication.Request.Profile;
using AnunciaPicos.Shared.Communication.Response.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("profile")]
    [ApiController]
    public class profileController : ControllerBase
    {
        [Authorize]
        [HttpGet("myprofile")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile([FromServices] IGetUserUseCase getUserUseCase)
        {
            ResponseGetProfileCommunication response = await getUserUseCase.Execute();
            return Ok(response);
        }

        [HttpGet("profile-specific/{id}")]
        [ProducesResponseType(typeof(ResponseGetProfileIdCommunication), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfileId([FromServices] IGetUserIdUseCase getUserByIdUseCase, int id)
        {
            ResponseGetProfileIdCommunication response = await getUserByIdUseCase.Execute(id);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("update-profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [RequestSizeLimit(10_000_000)] // Limita para 10MB por request (ajustável)
        public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase updateUserUseCase, [FromForm] RequestUpdateProfileCommunication request)
        {
            await updateUserUseCase.Execute(request);
            return NoContent();

        }

        [Authorize]
        [HttpDelete("delete-profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProfile([FromServices] IDeleteUserUseCase deleteUserUseCase)
        {
            await deleteUserUseCase.Execute();
            return NoContent();
        }
    }
}
