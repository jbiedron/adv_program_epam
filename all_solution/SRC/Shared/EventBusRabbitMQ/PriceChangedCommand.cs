using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
	public class PriceChangedCommand : ICommand
	{
        public int ProductId { get; private init; }
        public decimal NewPrice { get; private init; }
        public decimal OldPrice { get; private init; }

        public string CommandName => ICommand.PRICE_CHANGED_COMMAND;

		public PriceChangedCommand(int productId, decimal newPrice, decimal oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}
