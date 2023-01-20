using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD
{
    /*
    public static class ConfigureServices
    {
        public static IServiceCollection AddRabbitServices(this IServiceCollection services)
        {
            var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
            services Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
            services.AddTransient<IProductUpdateSender, ProductUpdateSender>();

            return services;
        }
    }
    */
}
