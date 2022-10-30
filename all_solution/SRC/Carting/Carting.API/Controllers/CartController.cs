using CartingService.BLL;
using CartingService.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CartingService.Controllers
{
    [Route("api")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartBO _cartBO;

        public CartController(CartBO cartBO)
        {
            _cartBO = cartBO;
        }

        /// <summary>
        /// Gets single cart item by key. Returns cart key and list of items.
        /// </summary>
        /// <param name="externalCartId"></param>
        /// <returns></returns>
        [HttpGet("v1/[controller]/{externalCartId}")]
        public async Task<ActionResult<Cart>> GetAsync([FromRoute] string externalCartId)
        {
            var cart = await _cartBO.GetAsync(externalCartId);
            return Ok(cart);
        }

        /// <summary>
        /// Gets single cart item by key. Returns list of cart items only.
        /// </summary>
        /// <param name="externalCartId"></param>
        /// <returns></returns>
        [HttpGet("v2/[controller]/{externalCartId}")]
        public async Task<ActionResult<List<CartItem>>> GetAsyncV2([FromRoute] string externalCartId)
        {
            var cart = await _cartBO.GetAsync(externalCartId);
            return Ok(cart.Items);
        }


        /// <summary>
        /// Adds the cart item to cart.
        /// </summary>
        /// <param name="externalCartId"></param>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        [HttpPost("v1/[controller]/{externalCartId}")]
        public async Task<ActionResult<int>> AddCartItem([FromRoute] string externalCartId, CartItemBO cartItem)
        {
            var result = await _cartBO.AddToCart(externalCartId, cartItem);
            return Ok(result);       
        }

        /// <summary>
        /// Removes cart item from the cart.
        /// </summary>
        /// <param name="externalCartId"></param>
        /// <param name="externalCartItemId"></param>
        /// <returns></returns>
        [HttpDelete("v1/[controller]/{externalCartId}/{externalCartItemId}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] string externalCartId, [FromRoute] string externalCartItemId)
        {
            var result = await _cartBO.RemoveFromCart(externalCartId, externalCartItemId);
            return Ok(result);
        }
    }
}
