using Application.Categories.Query;
using Application.Category.Command;

using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<List<Domain.Entities.Category>>> Get()
        {
            var results = await _mediator.Send(new GetAllCategoriesQuery());
            return results;
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Entities.Category>> Get(int id)
        {
            var results = await _mediator.Send(new GetCategoryByIdQuery(id));
            return results;
        }
        
        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateCategoryCommand command)
        {
            return await _mediator.Send(command);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if (id != command.CategoryId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return Ok();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return Ok();
        }
    }
}
