using EventBusRabbitMQ.OLD.Core.v2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v2
{
    /// <summary>
    /// Wrapper class for handling persisten connection to rabbit shutdowns
    /// </summary>
    public class RabbitMQPersistentConnection : IEventBusPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;                     // should not be musted, internal use only!
                                                                                    //      private readonly ILogger<RabbitMQPersistentConnection> _logger;
        private readonly int _retryCount;
        private IConnection _connection;
        public bool Disposed;

        readonly object _syncRoot = new();

        public RabbitMQPersistentConnection(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {

            _connectionFactory = new ConnectionFactory
            {

                HostName = "localhost",// rabbitMqOptions.Value.Hostname,            // FIX IT !!!
                UserName = "guest",// rabbitMqOptions.Value.UserName,
                Password = "guest",// rabbitMqOptions.Value.Password 

            };

            // ProductsQueue

            _retryCount = rabbitMqOptions.Value.ConnectionRetryCount;

            // _connection = this._connectionFactory.CreateConnection();
        }

        /*              // connection factory is internal use only!
        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQPersistentConnection> logger, int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }
        */

        public bool IsConnected => _connection is { IsOpen: true } && !Disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Disposed = true;

            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                //          _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            //      _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (_syncRoot)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        //            _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory
                            .CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    //         _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    //           _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (Disposed) return;

            //           _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (Disposed) return;

            //        _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (Disposed) return;

            //          _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
