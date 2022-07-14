using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Application.Commands;
using OutboxPattern.Application.Queries;

namespace OutboxPattern.API.Controllers
{
    [ApiController]
    [Route("product")]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductQueries _queries;

        public ProductController(IMediator mediator, IProductQueries queries)
        {
            _mediator = mediator;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var result = await _queries.GetAllProductsAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductAsync(Guid id)
        {
            var result = await _queries.GetProductAsync(id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateProductCommand cmd)
        {
            var guid = await _mediator.Send(cmd);
            return Ok(guid);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderAsync(DeleteProductCommand cmd)
        {
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
