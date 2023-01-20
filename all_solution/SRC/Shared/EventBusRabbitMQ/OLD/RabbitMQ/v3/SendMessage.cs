using EventBusRabbitMQ.OLD.Core.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v3
{
    public class SendMessage : IMessage
    {
        public string Message { get; set; }
        public void SendForgetPasswordEmailCommand(string message)
        {
            Message = message;
        }
    }
}
