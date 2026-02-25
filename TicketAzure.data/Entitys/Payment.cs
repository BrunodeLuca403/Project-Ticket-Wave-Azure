using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Enum;

namespace TicketAzure.Core.Entitys
{
    public class Payment : BaseEntity
    {
    

        public string ExternalId { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public PaymentStatusType Status { get; set; }
        public string PixCopyAndPaste { get; set; } = string.Empty;   

    }
}
