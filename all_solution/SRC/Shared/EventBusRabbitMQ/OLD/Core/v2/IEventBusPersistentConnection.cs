using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.Core.v2
{
    public interface IEventBusPersistentConnection //: IDisposable                // this is rabbitmq only for now, remove IModel or CreateModel method !!
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
