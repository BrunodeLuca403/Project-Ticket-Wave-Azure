using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.application.Dto
{
    public class PaymentCreateDto
    {
        public string CustomerEmail { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public int QuantityTicket { get; set; }

    }
}
