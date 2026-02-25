using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.application.Dto;

namespace TicketAzure.application.Services.Interface
{
    public interface IEventService 
    {
        Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken);

        Task<Result<List<ListEventDto>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
