// See https://aka.ms/new-console-template for more information
using SBSender.Services;

Console.WriteLine("Hello, ServiceBus!!");

var connectionString = "Endpoint=sb://advtraningdemov1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=2H7GHrgBkVAFFZa4RNXAZ0WdptQEHoABWtDVHAsCJ9I=";      // TODO: thru appSettings
var queueName = "DEMO_QUEUE";

var queueService = new QueueService(connectionString);          // TODO: use DI
await queueService.SemdMessageAsync<string>("TEST_MSG_ADV_DEMO_V1", queueName);



