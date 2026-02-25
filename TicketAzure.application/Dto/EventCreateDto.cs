using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.application.Dto
{
    public class EventCreateDto
    {
        public EventCreateDto(string name, DateTime date, string location, string description, decimal price, int availableTickets)
        {
            Name = name;
            Date = date;
            Location = location;
            Description = description;
            Price = price;
            AvailableTickets = availableTickets;
        }

        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AvailableTickets { get; set; }
        public FileEvent File { get; set; }
    }

    public record FileEvent(string fileName, string base64);
}
