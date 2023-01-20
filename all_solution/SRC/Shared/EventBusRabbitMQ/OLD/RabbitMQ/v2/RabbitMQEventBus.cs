using EventBusRabbitMQ.OLD.Core.v2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v2              // FIX NAMESPACES !!!    USUNAC - LAST DAY !!
{
    public class RabbitMQEventBus : IEventBus
    {
        /*
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        */


        // pid = 2, catid=4

        const string BROKER_NAME = "catalog_event_bus_v2";
        const string QUEUE_NAME = "catalog_event_queue_v2";

        // const string AUTOFAC_SCOPE_NAME = "eshop_event_bus";

        private readonly IEventBusPersistentConnection _persistentConnection;

        //    private readonly ILogger<RabbitMQEventBus> _logger;
        //private readonly IEventBusSubscriptionsManager _subsManager;
        //private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;

        private IModel _consumerChannel;
        private IModel _channel;
        private string _queueName = "catalog_event_queue";


        // MaybeNullWhenAttribute is IEventBusSubscriptionsManager ? 

        public RabbitMQEventBus(IEventBusPersistentConnection persistentConnection) //, ILogger<RabbitMQEventBus> logger, /* IEventBusSubscriptionsManager subsManager,*/ string queueName = null, int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));

            //     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //    _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            //      _queueName = queueName;
            //     _consumerChannel = CreateConsumerChannel();
            //     _autofac = autofac;
            //      _retryCount = retryCount;
            //     _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;

            _channel = CreateChannel();
        }

        private IModel CreateChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();

                _channel = _persistentConnection.CreateModel();
                //    _channel.ExchangeDeclare(exchange: BROKER_NAME,
                //                        type: "direct");

                _channel.QueueDeclare(queue: QUEUE_NAME,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            }

            return _channel;
        }

        /*
        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

         //   _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: QUEUE_NAME,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
           //     _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }
        */

        private void StartBasicConsume()
        {
            //  _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                //    _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                //  _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            //_logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);



            // for now only single type of event -> pass pricemodel and invoke "handle" action on server  side!!
            /*
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

          //  if (_subsManager.HasSubscriptionsForEvent(eventName))
          //  {
          //      await using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
          //      var subscriptions = _subsManager.GetHandlersForEvent(eventName);
         //       foreach (var subscription in subscriptions)
                {
         //           if (subscription.IsDynamic)
                    {
         //               if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }*/
        }


        /*
        public RabbitMQEventBus(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
        }
        */

        public void Publish(IntegrationEvent @event)
        {
            // sends the event to service bus

            if (@event != null)
            {
                var json = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(exchange: BROKER_NAME, routingKey: QUEUE_NAME, basicProperties: null, body: body);
            }

            // publish logic goes here!
            // no need to know exact type to serialize, deserialize !!!

            // throw new NotImplementedException();
        }

        public void Subscribe<T>() where T : IntegrationEvent
        {
            // TODO !!!
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
