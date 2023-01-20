using Domain.Entities;
using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using EventBusRabbitMQ;
//using EventBusRabbitMQ.OLD.RabbitMQ.v2;

namespace Catalog.Messaging.Send.Sender
{
    // TO REMOVE !!!!!! 
    public class ProductUpdateSender : IProductUpdateSender
    { 

        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public ProductUpdateSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _queueName = rabbitMqOptions.Value.QueueName;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            CreateConnection();         // TODO: this should not be placed inside ctor!
        }

        public void SendProduct(Product product)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(product);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
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
    }
}
