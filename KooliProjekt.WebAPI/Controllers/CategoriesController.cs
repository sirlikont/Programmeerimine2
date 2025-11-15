using KooliProjekt.Application.Features.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListCategoriesQuery
            {
                Page = page,
                PageSize = pageSize
            };

            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetCategoryQuery { Id = id };
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
