using System.Text.Json;
using System.Text;
using Reclone_BackEnd.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Reclone_BackEnd.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;

        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(Image imageDetails)
        {
            await using var client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]);
            ServiceBusSender sender = client.CreateSender(_configuration["QueueName"]);

            var messageBody = JsonSerializer.Serialize(imageDetails);
            var messageBytes = Encoding.UTF8.GetBytes(messageBody);

            var message = new ServiceBusMessage(messageBytes)
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };

            await sender.SendMessageAsync(message);
        }
    }
}
