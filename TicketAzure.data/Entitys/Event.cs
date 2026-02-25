using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.Core.Entitys
{
    public class Event : BaseEntity
    {
        public Event(string name, DateTime date, string location, string description, decimal price, int availableTickets, string imagePath) : base()
        {
            Name = name;
            Date = date;
            Location = location;
            Description = description;
            Price = price;
            AvailableTickets = availableTickets;
            ImagePath = imagePath;
        }

        public string Name { get;  set; }
        public DateTime Date { get;  set; }
        public string Location { get;  set; }
        public string Description { get;  set; }
        public decimal Price { get;  set; }
        public int AvailableTickets { get;  set; }
        public string ImagePath { get;  set; } = null!;
    }
}
