using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Delete;
using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnunciaPicos.Backend.API.Controllers
{
    [Route("favorite")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> RegisterFavorite([FromRoute] int id, [FromServices] IRegisterFavoriteUseCase registerFavoriteUseCase)
        {
            await registerFavoriteUseCase.Execute(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite([FromRoute] int id, [FromServices] IDeleteFavoriteUseCase deleteFavoriteUseCase)
        {
            await deleteFavoriteUseCase.Execute(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorite([FromServices] IGetFavoriteUseCase getFavoriteUseCase)
        {
            var products = await getFavoriteUseCase.Execute();
            return Ok(products);
        }

    }
}
