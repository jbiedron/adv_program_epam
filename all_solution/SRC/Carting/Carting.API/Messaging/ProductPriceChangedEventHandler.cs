using Carting.API.DTO;
using CartingService.DAL.Repository;
/*using EventBusRabbitMQ.Core.v2;*/

namespace Carting.API.Messaging
{
	/*
    public class ProductPriceChangedEventHandler : IIntegrationEventHandler<ProductPriceChangedEvent>
    {
        private readonly ICartingRespository _repository;

        public ProductPriceChangedEventHandler(ICartingRespository repository)
        {                  // TODO: pass logger !!
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(ProductPriceChangedEvent @event)
        {
            // get all the products from the cartItemId
            // update each product

            // externalId from cartItem is ProductId
            // await _repository.UpdateCartItemsByExternalId

            //await _repository.UpdateCartItemsByExternalId(cartItemToUpdate);
            await UpdatePriceInBasketItems(@event.ProductId, @event.OldPrice, @event.NewPrice, null);               // get all the carts from redis (before) and pass it to this method !!!
        }

        private async Task UpdatePriceInBasketItems(int productId, decimal oldPrice, decimal newPrice, CartDto cart)
        {
            Console.Write("TODO!");
            // this will be called for every cart from redis separately
        }
    }
    */
}
