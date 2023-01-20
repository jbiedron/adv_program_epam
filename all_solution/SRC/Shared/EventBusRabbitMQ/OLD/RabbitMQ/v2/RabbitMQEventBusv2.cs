using EventBusRabbitMQ.OLD.Core.v2;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v2
{
    public class RabbitMQEventBusv2 : IEventBus
    {
        //   private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private string _queueName = "Product_Price_Changed_Queue";
        private IModel _consumerChannel;

        public RabbitMQEventBusv2()
        {
            CreateConnection();         // TODO: this should not be placed inside ctor!
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",         // rabbitMqOptions.Value.Hostname,            // FIX IT !!!
                    UserName = "guest",             // rabbitMqOptions.Value.UserName,
                    Password = "guest",             // rabbitMqOptions.Value.Password 
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
                return true;

            CreateConnection();
            return _connection != null;
        }


        public void Publish(IntegrationEvent @event)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        public void Subscribe<T>() where T : IntegrationEvent
        {
            if (ConnectionExists())
            {
                _consumerChannel = _connection.CreateModel();
                _consumerChannel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_consumerChannel);
                consumer.Received += (ch, ea) =>
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                    /*
                    var productModel = JsonConvert.DeserializeObject<ProductModel>(content);         // product is not exact version of catalog product
                    HandleMessage(productModel);*/

                    _consumerChannel.BasicAck(ea.DeliveryTag, false);
                };
            }

            /*
            using (var channel = _connection.CreateModel()) { 
            
            }
            var consumerChannel = _connection.CreateModel();

            consumerChannel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            */

            // _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        /*
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }
        */
    }
}
