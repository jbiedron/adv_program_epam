using Application.Products.Query;
using Application.Products.Command;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var results = await _mediator.Send(new GetAllProductsQuery());
            return results;

        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            var results = await _mediator.Send(new GetProductByIdQuery(id));
            return results;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.ProductId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return Ok();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok();
        }
    }
}
