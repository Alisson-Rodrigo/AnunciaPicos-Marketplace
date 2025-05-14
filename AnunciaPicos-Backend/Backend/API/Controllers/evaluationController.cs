using AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Register;
using AnunciaPicos.Shared.Communication.Request.Evaluation;
using AnunciaPicos.Shared.Communication.Response.Evaluation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("evaluation")]
    [ApiController]
    public class evaluationController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostEvaluation([FromBody] RequestEvaluationCommunication request, [FromServices] IPostEvaluationUseCase pushEvaluationUseCase)
        {
            await pushEvaluationUseCase.Execute(request);
            return Created("EvaluationController", null);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseGetEvaluationCommunicattion), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvaluation([FromRoute] int id, [FromServices] IGetEvaluationUseCase getEvaluationUseCase)
        {
            var evaluations = await getEvaluationUseCase.Execute(id);
            return Ok(evaluations);
        }

        [HttpGet("average/{id}")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAverageEvalution([FromRoute] int id, IGetEvaluationUseCase getEvaluationUseCase)
        {
            double evaluations = await getEvaluationUseCase.ExecuteAverage(id);
            return Ok(evaluations);
        }


    }
}
