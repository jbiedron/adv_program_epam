
using Carting.API.DTO;
using CartingService.Service;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]                                     // Keep the existing route serving a default version (backward compatible).
   // [Route("api/v{version:apiVersion}/[controller]")]             // uncommenting this will stop swagger from working - conflict in mapping
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{externalCartId}")]
        public async Task<ActionResult<CartDto>> GetAsync([FromRoute] string externalCartId)
        {
            var cart = await _cartService.GetAsync(externalCartId);
            return Ok(cart);
        }

        [HttpPost("{externalCartId}")]
        public async Task<ActionResult<int>> AddCartItem([FromRoute] string externalCartId, CartItemDto cartItem)
        {
            var result = await _cartService.AddToCart(externalCartId, cartItem);
            return Ok(result);       
        }

        [HttpDelete("{externalCartId}/{externalCartItemId}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] string externalCartId, [FromRoute] string externalCartItemId)
        {
            var result = await _cartService.RemoveFromCart(externalCartId, externalCartItemId);
            return Ok(result);
        }
    }
}
