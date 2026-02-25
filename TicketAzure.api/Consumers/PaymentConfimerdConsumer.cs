
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs.Models;
using TicketAzure.application.Services.Interface;
using static TicketAzure.application.Services.PaymentService;

namespace TicketAzure.api.Consumers
{
    public class PaymentConfimerdConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;

        public PaymentConfimerdConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceBusClient = new ServiceBusClient(_configuration["ServiceBusConnection"], new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            await using var processor = serviceBusClient.CreateProcessor("payment-confirmed", new ServiceBusProcessorOptions());


            processor.ProcessMessageAsync += ProcessMessage;
            processor.ProcessErrorAsync += ProcessError;

            await processor.StartProcessingAsync(stoppingToken);
            try
            {

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                await processor.StopProcessingAsync();

            }
        }

        private async Task ProcessMessage(ProcessMessageEventArgs args)
        {
            var scope = _serviceProvider.CreateScope();

            var processOrderService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            try
            {
                var messageBody = args.Message.Body.ToString();

                var paymentConfirm = System.Text.Json.JsonSerializer.Deserialize<PaymentPaidEvent>(messageBody);

                if (paymentConfirm == null)
                {
                    Console.WriteLine("Invalid message format.");
                    await args.AbandonMessageAsync(args.Message);
                    return;
                }

                await processOrderService.ProcessPayment(paymentConfirm);

                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await args.DeadLetterMessageAsync(args.Message);

            }
        }

        private async Task ProcessError(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            await Task.CompletedTask;

        }
    }
}
