using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventBusRabbitMQ.OLD
{
    // TODO: remove !!!!
    public class ProductUpdateReceiver //: //BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        //  private IConnect
        // private readonly ICartingRespository _repository;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
    }
}
