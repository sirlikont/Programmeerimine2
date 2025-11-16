using KooliProjekt.Application.Features.OrderItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListOrderItemsQuery
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetOrderItemQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveOrderItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteOrderItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
