using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketAzure.Core.Enum;

namespace TicketAzure.application.Dto
{
    public class PaymentResultDto
    {
        public Guid Id { get; set; }
        public string PixCopyAndPast { get; set; } = string.Empty;
        public PaymentStatusType Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
