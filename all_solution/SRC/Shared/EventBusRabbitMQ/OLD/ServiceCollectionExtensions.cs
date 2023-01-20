using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusRabbitMQ.OLD
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfigurationSection section)
        {
            /*
            var options = new RawRabbitConfiguration();
            section.Bind(options);

            var client = BusClientFactory.CreateDefault(options);
            services.AddSingleton<IBusClient>(_ => client);
            services.AddSingleton<IMyBusClient, MyBusClient>();
            */

            /*
            var client = BusClientFactory.CreateDefault(busConfig);
            services.AddSingleton<IBusClient>(_ => client);
            services.AddSingleton<IMyBusClient, MyBusClient>();*/

            // this will be added to the api/app project - handler implementation must have access to the db
            //      services.AddTransient<IMessageHandler<SendMessage>, SendMessageHandler>();

            /*
            // event bus - v4
            var config = new RawRabbit.Configuration.RawRabbitConfiguration
            {
                Username = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = 5672,
                Hostnames = new List<string> { "localhost" },
                RequestTimeout = TimeSpan.FromSeconds(10),
                PublishConfirmTimeout = TimeSpan.FromSeconds(1),
                RecoveryInterval = TimeSpan.FromSeconds(1),
                PersistentDeliveryMode = true,
                AutoCloseConnection = true,
                AutomaticRecovery = true,
                TopologyRecovery = true,
                Exchange = new RawRabbit.Configuration.GeneralExchangeConfiguration
                {
                    Durable = true,
                    AutoDelete = false,
                    Type = RawRabbit.Configuration.Exchange.ExchangeType.Topic
                },
                Queue = new RawRabbit.Configuration.GeneralQueueConfiguration
                {
                    Durable = true,
                    AutoDelete = false,
                    Exclusive = false
                }
            };
            services.AddSingleton<IEventPublisher, RabbitEventPublisher>();
            */
        }
    }
}
