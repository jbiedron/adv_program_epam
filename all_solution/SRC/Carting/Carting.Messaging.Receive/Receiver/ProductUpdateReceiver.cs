using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Carting.Messaging.Receive.Receiver
{
    private IModel _channel;
    private IConnection _connection;
    private readonly ICustomerNameUpdateService _customerNameUpdateService;
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;

    internal class ProductUpdateReceiver : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            return Task.CompletedTask;
        }
    }
}
