using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Delete;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.GetId;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Search;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Update;
using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Communication.Response.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {
        [Authorize]
        [HttpPost("cadastro")]
        [RequestSizeLimit(10_000_000)] // Limita para 10MB por request (ajustável)
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterProduct([FromForm] RequestRegisterProductCommunication request, [FromServices] IRegisterProductUseCase registerProductUseCase)
        {
            await registerProductUseCase.Execute(request);
            return Created("ProductController", null);
        }

        [HttpGet("exibicao-produtos")]
        [ProducesResponseType(typeof(List<ResponseProductGetCommunication>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromServices] IGetProductUseCase getProductsUseCase)
        {
            List<ResponseProductGetCommunication> response = await getProductsUseCase.Execute();
            return Ok(response);
        }

        [HttpGet("exibicao-produtos/{id}")]
        [ProducesResponseType(typeof(ResponseProductGetCommunication), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProduct([FromRoute] int id, [FromServices] IGetProductIdUseCase getProductsUseCase)
        {
            var response = await getProductsUseCase.Execute(id);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("atualizar-produtos/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id,[FromBody] RequestUpdateProductCommunication request, [FromServices] IUpdateProductUseCase updateProductUseCase)
        {
            await updateProductUseCase.Execute(request, id);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("deletar-produtos/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id, [FromServices] IDeleteProductUseCase deleteProductUseCase)
        {
            await deleteProductUseCase.Execute(id);
            return NoContent();
        }

        [HttpGet("busca/produtos")]
        [ProducesResponseType(typeof(List<ResponseProductSearchCommunication>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] RequestProductSearchCommunication request, [FromServices] ISearchAndFiltersUseCase searchAndFiltersUseCase)
        {
            var response = await searchAndFiltersUseCase.ExecuteSearch(request);
            return Ok(response);
        }

        [HttpGet("busca/autocomplete")]
        public async Task<IActionResult> AutoComplete([FromQuery] string term, ISearchAndFiltersUseCase searchAndFiltersUseCase)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Termo de busca obrigatório.");

            var response = await searchAndFiltersUseCase.ExecuteAutoComplete(term);
            return Ok(response);
        }

    }
}
