using Carting.API.DTO;
using CartingService.Service;
using Microsoft.AspNetCore.Mvc;

namespace Carting.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartController : ControllerBase                // creating 2nd controller was the only way to mave versioning.mvc package working 
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{externalCartId}")]
        public async Task<ActionResult<List<CartItemDto>>> GetAsyncV2([FromRoute] string externalCartId)
        {
            var cart = await _cartService.GetAsync(externalCartId);
            return Ok(cart.Items);
        }
    }
}
