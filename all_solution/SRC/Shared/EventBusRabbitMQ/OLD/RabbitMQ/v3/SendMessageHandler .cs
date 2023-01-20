using EventBusRabbitMQ.OLD.Core.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v3
{
    public class SendMessageHandler : IMessageHandler<SendMessage>
    {
        public async Task HandleAsync(SendMessage @event, CancellationToken token)
        {
            Console.WriteLine($"Receive: {@event.Message}");
            //  return Task.CompletedTask;
        }
    }
}
