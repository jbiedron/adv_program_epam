using CartingService.DAL.Entities;

namespace Carting.API.DTO
{
    public class CartDto
    {
        public CartDto(Cart entity)
        {
            this.ExternalId = entity.ExternalId;
            if (entity.Items != null)
                entity.Items.ForEach(x => {
                    this.Items.Add(new CartItemDto(x));
                });
        }

        public string ExternalId { get; private set; }
        public List<CartItemDto> Items { get; private set; } = new List<CartItemDto>();
    }
}
