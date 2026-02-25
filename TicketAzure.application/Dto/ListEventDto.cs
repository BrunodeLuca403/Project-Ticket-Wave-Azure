using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.application.Dto
{
    public class ListEventDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string fileName { get; set; }
        public DateTime createdAt { get; set; }
    }
}
