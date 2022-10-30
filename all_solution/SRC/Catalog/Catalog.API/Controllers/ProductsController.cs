using Application.Products.Query;
using Application.Products.Command;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.App.Products.Query;
using Catalog.App.Common;

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

        /// <summary>
        /// Gets list of products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var results = await _mediator.Send(new GetAllProductsQuery());
            return results;

        }

        /// <summary>
        /// Gets single product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var results = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(results);
        }

        /// <summary>
        /// Get products by category. Returns paged results with default page size = 20
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pagedQueryRequest"></param>
        /// <returns></returns>
        [HttpGet("{categoryId}/products")]
        public async Task<ActionResult> GetProductsByCategoryList([FromRoute] int categoryId, [FromQuery] PagedQueryRequest pagedQueryRequest) 
        {
            var byCategoryPaged = new GetProductsByCategoryPagedQuery(categoryId, pagedQueryRequest.PageNo, pagedQueryRequest.PageSize);

            var results = await _mediator.Send(byCategoryPaged);
            return Ok(results);
        }

        /// <summary>
        /// Creates product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Updates product item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, UpdateProductCommand command)
        {
            if (id != command.ProductId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Removes product item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok();
        }
    }
}
