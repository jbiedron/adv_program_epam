using EventBusRabbitMQ.OLD.Core.v3;

namespace EventBusRabbitMQ.OLD.Catalog.v3
{
    public class ProductPriceChangedMessage : IMessage
    {
        public int ProductId { get; private set; }
        public decimal NewPrice { get; private set; }
        public decimal OldPrice { get; private set; }

        public ProductPriceChangedMessage(int productId, decimal newPrice, decimal oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}
