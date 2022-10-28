using CartingService.BLL;
using CartingService.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartBO _cartBO;

        public CartController(CartBO cartBO)
        {
            _cartBO = cartBO;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetAsync(string externalCartId)
        {
            var cart = await _cartBO.GetAsync(externalCartId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddCartItem(string externalCartId, CartItemBO cartItem)
        {
            var result = await _cartBO.AddToCart(externalCartId, cartItem);
            return Ok(result);       
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteCartItem(string externalCartId, string externalCartItemId)
        {
            var result = await _cartBO.RemoveFromCart(externalCartId, externalCartItemId);
            return Ok(result);
        }
    }
}
