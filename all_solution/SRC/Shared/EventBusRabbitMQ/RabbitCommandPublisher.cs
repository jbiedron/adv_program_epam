using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class RabbitCommandPublisher : ICommandPublisher
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public RabbitCommandPublisher(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
			_queueName = "ProductsQueue_LAST";
			_hostname = "localhost";
			_username = "guest";
			_password = "guest";

			CreateConnection();
        }

        public async Task PublishAsync(ICommand command)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(command);
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
