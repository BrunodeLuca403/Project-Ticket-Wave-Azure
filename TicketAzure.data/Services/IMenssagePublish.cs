using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.Core.Services
{
    public interface IMenssagePublish
    {
        Task PublishAsync(string queue, string message, CancellationToken cancellationToken); 
    }
}
