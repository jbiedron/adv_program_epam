// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

var connectionString = "XXX";      // TURN OFF WHEN PUSHED TO GIT !!!!!!
var queueName = "DEMO_QUEUE";

Console.WriteLine("Hello, Receiver!");

var queueClient = new QueueClient(connectionString, queueName);
var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceiveHandler)
{
    MaxConcurrentCalls = 1,
    AutoComplete = false
};
queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
Console.ReadLine();
await queueClient.CloseAsync();     


async Task ProcessMessageAsync(Message message, CancellationToken token)
{
    var jsonString = Encoding.UTF8.GetString(message.Body);
    var item = JsonSerializer.Deserialize<string>(jsonString);
    Console.WriteLine($"Item received: {item}");

    await queueClient.CompleteAsync(message.SystemProperties.LockToken);        // because autocomplete is OFF
}

Task ExceptionReceiveHandler(ExceptionReceivedEventArgs arg)
{
    Console.WriteLine($"Message handler exception: {arg.Exception}");
    return Task.CompletedTask;
}


