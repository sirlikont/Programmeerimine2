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

        // LIST with pagination
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

            // Return full OperationResult so integration tests can deserialize
            return Ok(result);
        }

        // GET single order item by Id
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetOrderItemQuery { Id = id };
            var response = await _mediator.Send(query);

            // Return 404 if nothing found or has errors
            if (response == null || response.Value == null || response.HasErrors)
                return NotFound();

            return Ok(response);
        }

        // CREATE or UPDATE order item
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveOrderItemCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // DELETE order item
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteOrderItemCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
