using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public interface ICommand
    {
        public static string PRICE_CHANGED_COMMAND = "price_changed_command";

		public string CommandName { get; }
    }
}
