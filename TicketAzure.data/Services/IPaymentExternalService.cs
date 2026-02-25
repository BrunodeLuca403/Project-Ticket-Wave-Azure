using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Dto.CreateEvent;

namespace TicketAzure.Core.Services
{
    public interface IPaymentExternalService
    {
        Task<ResponseEfi?> GeneratePixAsync(decimal value);

    }
}
