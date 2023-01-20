using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.OLD.RabbitMQ.v2
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public string QueueName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int ConnectionRetryCount { get; set; } = 5;
    }
}
