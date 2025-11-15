using KooliProjekt.Application.Features.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // LIST with pagination
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListOrdersQuery
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        // GET single order by Id
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetOrderQuery { Id = id };
            var response = await _mediator.Send(query);
            return Ok(response.Value);
        }

        // SAVE order (create or update)
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // DELETE order
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
