using CartingService.DAL.Entities;

namespace Carting.API.DTO
{
    public class CartItemDto
    {
        public CartItemDto(CartItem entity)
        {
            this.ExternalId = entity.ExternalId;
            this.Name = entity.Name;
            this.ImageUrl = entity.ImageUrl;
            this.Price = entity.Price;
            this.Quantity = entity.Quantity;
        }

        public string ExternalId { get; private set; }

        public string Name { get; private set; }

        public string ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public uint Quantity { get; private set; }
    }
}
