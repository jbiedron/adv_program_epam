using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBSender.Services
{
    public class QueueService
    {
        private string _connectionString = null;
        private QueueClient queueClient = null;

        public QueueService(string connectionString) { 
             // TODO: pass IConfiguration  and appSettings
            this._connectionString = connectionString;
          //   this.queueClient = new QueueClient(connectionString, queueName);
        }

        public async Task SemdMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            var queueClient = new QueueClient(this._connectionString, queueName);
            string msgBody = JsonSerializer.Serialize(serviceBusMessage);

            var message = new Message(Encoding.UTF8.GetBytes(msgBody));
            await queueClient.SendAsync(message);

        }

        public async Task ReceiveMessageAsync(string queueName)
        {
            var queueClient = new QueueClient(this._connectionString, queueName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceiveHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            Console.ReadLine();
            await queueClient.CloseAsync();
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            var jsonString = Encoding.UTF8.GetString(message.Body);
            var item = JsonSerializer.Deserialize<string>(jsonString);
            Console.WriteLine($"Item received: { item }");
           
        }

        private Task ExceptionReceiveHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}
