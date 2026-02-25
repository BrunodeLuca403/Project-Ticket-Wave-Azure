using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.Core.Entitys
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsDelete = false;
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDelete { get; set; }


        public void SetAsDelete()
        {
            IsDelete = true;
        }
    }
}
