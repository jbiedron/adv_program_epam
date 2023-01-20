using EventBusRabbitMQ.OLD.Core.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.Catalog.v2
{
    // TODO: move it to the concrete services? (we need them in both for serialization/deserialization)
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; private init; }
        public decimal NewPrice { get; private init; }
        public decimal OldPrice { get; private init; }

        public ProductPriceChangedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}
