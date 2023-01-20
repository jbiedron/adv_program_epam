using EventBusRabbitMQ.OLD.Core.v3;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v3
{
    public class MyBusClient : IMyBusClient
    {
        private readonly IBusClient _bus;

        public MyBusClient(IBusClient bus)
        {
            _bus = bus;
        }

        public async Task Publish(IMessage t)
        {
            await _bus.PublishAsync(t);
        }
    }
}