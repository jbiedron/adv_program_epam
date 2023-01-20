using Application.Common;
//using Catalog.App.EventBus;
using Catalog.Messaging.Send.Sender;
using EventBusRabbitMQ;
//using EventBusRabbitMQ.OLD.RabbitMQ.v3;
//using EventBusRabbitMQ.RabbitMQ.v3;
using MediatR;


namespace Application.Products.Command
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public uint Amount { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IProductUpdateSender _productUpdateSender;
        private readonly ICommandPublisher _commandPublisher;

        //  private readonly IProductPriceUpdateSender _productPriceUpdateSender;
        //  private readonly IMyBusClient _myBusClient;

        public UpdateProductCommandHandler(IApplicationDbContext context, IProductUpdateSender productUpdateSender
            /*, IProductPriceUpdateSender productPriceUpdateSender, IMyBusClient myBusClient*/, ICommandPublisher commandPublisher)
        {
            this._context = context;
            this._productUpdateSender = productUpdateSender;
			//    this._productPriceUpdateSender = productPriceUpdateSender;
			//    this._myBusClient = myBusClient;

			this._commandPublisher = commandPublisher;

		}

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Products
                .FindAsync(new object[] { request.ProductId }, cancellationToken);

            if (entity == null)
                throw new ArgumentException("product not found.");

            var category = await _context.Categories
              .FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (category == null)
                throw new ArgumentException("category not found.");

            //   var triggerEventRoCartService = false;
            var priceDifferent = entity != null && entity.Price != request.Price;
			if (priceDifferent)
			{
				var oldPrice = entity.Price;
				var newPrice = request.Price;

				/*
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(request.ProductId, oldPrice, newPrice);
                _productPriceUpdateSender.SendProductPriceUpdate(priceChangedEvent);
                */

				var cmd = new PriceChangedCommand(entity.ProductId, newPrice, oldPrice);
				await this._commandPublisher.PublishAsync(cmd);


             //    var message = new SendMessage();
            //     var message = "TEST_RABBIT_RAW !!";

          //      await _myBusClient.Publish(message);
            }


            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Image = request.ImageUrl;
            entity.Price = request.Price;
            entity.Amount = request.Amount;

            entity.Category = category;

            await _context.SaveChangesAsync(cancellationToken);

            // check if the price is diffrent, is so triver event
            // send to eventbus
            _productUpdateSender.SendProduct(entity);
            

            return Unit.Value;
        }
    }
}