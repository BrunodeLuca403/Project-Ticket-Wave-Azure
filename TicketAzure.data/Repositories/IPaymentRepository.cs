using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Entitys;

namespace TicketAzure.Core.Repositories
{
    public interface IPaymentRepository
    {
        Task CreatePaymentAsync(Payment payment, CancellationToken cancellationToken);
        Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Payment> GetByExternalIdAsync(string ExternalId, CancellationToken cancellationToken);
        Task UpdateAsync(Payment payment, CancellationToken cancellationToken);

    }
}
