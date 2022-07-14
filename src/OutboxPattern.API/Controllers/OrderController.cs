using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Application.Commands;
using OutboxPattern.Application.Queries;

namespace OutboxPattern.API.Controllers
{
    [ApiController]
    [Route("order")]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _queries;

        public OrderController(IMediator mediator, IOrderQueries queries)
        {
            _mediator = mediator;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var result = await _queries.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderAsync(Guid id)
        {
            var result = await _queries.GetOrderAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderCommand cmd)
        {
            var guid = await _mediator.Send(cmd);
            return Ok(guid);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderAsync(DeleteOrderCommand cmd)
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
