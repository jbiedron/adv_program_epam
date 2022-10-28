using CartingService.DAL.Entities;
using CartingService.DAL.Repository;

namespace CartingService.BLL
{
    /// <summary>
    /// Business Object for carting service.
    /// </summary>
    public class CartBO
    {
        private readonly ICartingRespository _repository;

        public CartBO(ICartingRespository repository) 
        {
            _repository = repository;
        }

        public async Task<Cart> GetAsync(string externalId) {
            return await _repository.GetAsync(externalId);
        }

        public async Task<int?> AddToCart(string externalId, CartItemBO cartItemBO) 
        {
            // we could use automapper for this
            // some additional  validation might take plase, like when adding the same item updates only quantity

            if (cartItemBO?.ExternalId is null)
                throw new ArgumentNullException(nameof(cartItemBO.ExternalId));

            if (cartItemBO?.Name is null)
                throw new ArgumentNullException(nameof(cartItemBO.Name));

            if (cartItemBO.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(cartItemBO.Quantity));

            // if no cart with given key - create new
            var cart = await _repository.GetAsync(externalId) ?? new Cart(externalId);
           
            var itemToAdd = new CartItem
            {
                ExternalId = cartItemBO.ExternalId,
                Name = cartItemBO.Name,
                ImageUrl = cartItemBO.ImageUrl,
                Quantity = cartItemBO.Quantity,
                Price = cartItemBO.Price,
            };
            cart.Items.Add(itemToAdd);

            await _repository.UpdateAsync(cart);
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalCartId"></param>
        /// <param name="externalCartItemId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int?> RemoveFromCart(string externalCartId, string externalCartItemId ) {

            var cart = await _repository.GetAsync(externalCartId);
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            // look for the item to remove
            var toRemove = cart.Items.FirstOrDefault(x => x.ExternalId == externalCartItemId);      // TODO: whitespace trimming if needed
            if (toRemove == null)
                throw new ArgumentNullException(nameof(toRemove));

            cart.Items.Remove(toRemove);

            await _repository.UpdateAsync(cart);
            return 1;
        }
    }
}
