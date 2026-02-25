using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Services;

namespace TicketAzure.infrastructure.Services
{
    public class MenssagePublish : IMenssagePublish
    {
        private readonly IConfiguration? _configuration;
        private readonly ServiceBusClient _serviceBusClient;

        public MenssagePublish(IConfiguration? configuration, ServiceBusClient serviceBusClient)
        {
            _configuration = configuration;
            _serviceBusClient = new ServiceBusClient(configuration["ServiceBusConnection"]);
        }
        public async Task PublishAsync(string queue, string message, CancellationToken cancellationToken)
        {
             ServiceBusSender sender = _serviceBusClient.CreateSender(queue);
             ServiceBusMessage busMessage = new ServiceBusMessage(message);
             await sender.SendMessageAsync(busMessage, cancellationToken);
        } 
     
    }
}
