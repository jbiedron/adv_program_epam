using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace EventBusRabbitMQ
{
	public abstract class RabbitCommandSubscriber : ICommandSubscriber
    {
        private IModel _channel;
        private IConnection _connection;

        public delegate void OnCommandReceived(string json);  // subject is command
        public event OnCommandReceived CommandReceived;

        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public RabbitCommandSubscriber() 
        {
            _queueName = "ProductsQueue_LAST";
            _hostname = "localhost";
            _username = "guest";
            _password = "guest";

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(content);

                CommandReceived?.Invoke(content);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);
        }

        // this is abstract method to implement on the client side
        public abstract void Subscribe(ICommand command);


        /*
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var productModel = JsonConvert.DeserializeObject<ProductModel>(content);         // product is not exact version of catalog product
                HandleMessage(productModel);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            await Task.Delay(5000);
        }
    }
        */
        /*
    private void HandleMessage(ProductModel productModel)
    {
        /*
        // if nothing changed in productmodel the property's value will be null
        var cartItemToUpdate = new CartItem()
        {
            ExternalId = productModel.ProductId + "",
            Name = productModel.Name,
            ImageUrl = productModel.Image,
            Price = productModel.Price,
            Description = productModel.Description
        };

        _repository.UpdateCartItemsByExternalId(cartItemToUpdate);

    }
    */

        private void OnConsumerCancelled(object? sender, ConsumerEventArgs e)
        {
            // nothing to do...
        }

        private void OnConsumerRegistered(object? sender, ConsumerEventArgs e)
        {
            // nothing to do...
        }

        private void OnConsumerUnregistered(object? sender, ConsumerEventArgs e)
        {
            // nothing to do...
        }

        private void OnConsumerShutdown(object? sender, ShutdownEventArgs e)
        {
            // nothing to do...
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            // nothing to do...
        }

		/* - UNCOMMENT - LAST VERSION !!!!!
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
        */
	}
}
