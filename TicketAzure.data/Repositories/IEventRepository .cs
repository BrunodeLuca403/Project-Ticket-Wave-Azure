using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Entitys;

namespace TicketAzure.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(Event @event, CancellationToken cancellationToken);
        Task UpdateAsync(Event @event);
    }
}
