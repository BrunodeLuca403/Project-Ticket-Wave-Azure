using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketAzure.Core.Repositories;
using TicketAzure.Core.Services;
using TicketAzure.infrastructure.Repositories;
using TicketAzure.infrastructure.Services;
using TicketAzure.infrastructure.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace TicketAzure.infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(x => new CosmosClient(connectionString: configuration["CosmosDbConnection"]));


            services.AddStorageService(configuration);
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IPaymentExternalService, PaymentExternalService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();


            services.Scan(scan => scan
                .FromAssemblies(typeof(InfrastructureModule).Assembly)
                .AddClasses(classes => classes.AssignableTo<IEventRepository>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.AddScoped(typeof(IMenssagePublish), typeof(MenssagePublish));

            return services;
        }

        private static void AddStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            var storageSection = configuration.GetRequiredSection("StorageSettings")!;

            services.Configure<StorageSettings>(options => storageSection.Bind(options));

            var storageSettings = storageSection.Get<StorageSettings>()!;

            var blobClient = new BlobServiceClient(storageSettings.ConnectionString);

            services.AddScoped<IStorageService, StorageService>();
            
            services.AddSingleton(blobClient);
        }
    }
}
