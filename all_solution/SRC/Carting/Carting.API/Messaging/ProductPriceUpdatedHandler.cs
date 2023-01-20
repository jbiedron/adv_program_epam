using CartingService.DAL.Repository;
using EventBusRabbitMQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Carting.API.Messaging
{
	public class ProductPriceUpdatedHandler : RabbitCommandSubscriber //,  IProductPriceUpdatedHandler
    {
        private readonly ICartingRespository _repository;
        public ProductPriceUpdatedHandler(ICartingRespository repository)
        {
            this._repository = repository;
            this.CommandReceived += ProductPriceUpdatedHandler_CommandReceived;

        }

        /*
        public async Task Handle(string json)
        {
            // parse to command
            //  var itemReceived = JsonConvert.DeserializeObject<ProductModel>(json);


            // PO KLASIE BASOWEJ, SUBSCRIBE ZAIMPLEMENTOWAC TYLKO handler to delegaty zamiast subscribe metoda handle !!!!
            // W tej klasie tylko jedna metoda !!!!! reszta dziala jako singletom w projekcie sharowym !!!


            var itemReceived = JsonConvert.DeserializeObject<PriceChangedCommand>(json);
            _repository.UpdateCartItemsByExternalId_v2(itemReceived);
        }
        */

        public override void Subscribe(ICommand command)                    // TODO: remove it?!?!
		{
			// add event handler 
		// 	this.CommandReceived += ProductPriceUpdatedHandler_CommandReceived;
		}

		private void ProductPriceUpdatedHandler_CommandReceived(string json)
		{
            var itemReceived = JsonConvert.DeserializeObject<PriceChangedCommand>(json);
            _repository.UpdateCartItemsByExternalId_v2(itemReceived);
        }
	}
}
