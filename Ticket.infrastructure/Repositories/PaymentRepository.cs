using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Entitys;
using TicketAzure.Core.Repositories;

namespace TicketAzure.infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {

        private readonly Container _container;

        public PaymentRepository(CosmosClient cosmosClient)
        {
            _container = cosmosClient.GetContainer("ticket-db", "events");
        }

        public async Task CreatePaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _container.CreateItemAsync(payment, cancellationToken: cancellationToken);
        }

        public async Task<Payment> GetByExternalIdAsync(string ExternalId, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.ExternalId = @id")
                             .WithParameter("@id", ExternalId);

            var iterator = _container.GetItemQueryIterator<Payment>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                 .WithParameter("@id", id.ToString());

            var iterator = _container.GetItemQueryIterator<Payment>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();   
            }

            return null;
        }

        public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _container.ReplaceItemAsync<Payment>(payment, payment.Id.ToString(), cancellationToken: cancellationToken);
        }
    }
}
