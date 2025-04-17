using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        [HttpPost("cadastro")]
        [ProducesResponseType(typeof(ResponseRegisterCommunication), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterUser([FromBody] RequestRegisterCommunication registerModel, [FromServices] IRegisterUseCase registerUseCase)
        {
            var response = await registerUseCase.Execute(registerModel);
            return Created("AuthController", response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseLoginCommunication), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginUser([FromBody] RequestLoginCommunication loginModel, [FromServices] ILoginUseCase loginUseCase)
        {
            var response = await loginUseCase.Execute(loginModel);
            return Ok(response);
        }

        [HttpPost("esqueceu-senha")]
        public async Task<IActionResult> UpdatePassword([FromServices] IUpdatePasswordUseCase updatePassUseCase, [FromBody] RequestUpdatePasswordCommunication updatePasswordModel)
        {
            await updatePassUseCase.Execute(updatePasswordModel);
            return Ok();
        }

        [HttpPost("validar-recuperacao")]
        public async Task<IActionResult> ValidateRecovery([FromServices] IResetUseCase resetUseCase, RequestResetPasswordCommunication resetRequest)
        {
            await resetUseCase.Execute(resetRequest);
            return Ok();
        }

    }
}
