using CartingService.DAL.Entities;

namespace CartingService.DAL.Repository
{
    public interface ICartingRespository
    {
        // this gets entire basket with items
        public Task<Cart> GetAsync(string id);
        
        // this update entite basket (when item is added/removed)
        public Task<Cart> UpdateAsync(Cart toUpdate);

        public void UpdateCartItemsByExternalId(CartItem cartItem);
    }
}
