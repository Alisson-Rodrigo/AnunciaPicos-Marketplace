﻿using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.AuthFacebook;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class authController : ControllerBase
    {
        [HttpPost("register")]
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

        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePassword([FromServices] IUpdatePasswordUseCase updatePassUseCase, [FromBody] RequestUpdatePasswordCommunication updatePasswordModel)
        {
            await updatePassUseCase.Execute(updatePasswordModel);
            return Ok();
        }

        [HttpPost("validation-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateRecovery([FromServices] IResetUseCase resetUseCase, RequestResetPasswordCommunication resetRequest)
        {
            await resetUseCase.Execute(resetRequest);
            return Ok();
        }

        [HttpPost("facebook-login")]
        [ProducesResponseType(typeof(ResponseFacebookLoginCommunication), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FacebookLogin(
            [FromBody] RequestFacebookLoginCommunication facebookModel,
            [FromServices] IFacebookLoginUseCase facebookLoginUseCase)
        {
            try
            {
                var response = await facebookLoginUseCase.Execute(facebookModel);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
