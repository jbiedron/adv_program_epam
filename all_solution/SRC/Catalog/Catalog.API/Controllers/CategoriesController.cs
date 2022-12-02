using Application.Categories.Query;
using Application.Category.Command;
using Catalog.App.Categories.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //  [Authorize(Roles = "Buyer")]
    //   [Authorize(Policy = "BuyerRead")]
    //   [Authorize(Policy = "ManagerFull")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        /// <summary>
        /// Gets list of categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> Get()
        {
            var results = await _mediator.Send(new GetAllCategoriesQuery());
            return results;
        }

        /// <summary>
        /// Gets single category item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var results = await _mediator.Send(new GetCategoryByIdQuery(id));
            return results;
        }
        
        /// <summary>
        /// Creates category.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ManagerFull")]
        public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Updates category data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerFull")]
        public async Task<ActionResult> Update([FromRoute] int id, UpdateCategoryCommand command)
        {
            if (id != command.CategoryId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Removes category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerFull")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return Ok();
        }
    }
}
