using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.Core.v3
{
    public interface IMessageHandler<in T> where T : IMessage
    {
        Task HandleAsync(T message, CancellationToken token);
    }
}
