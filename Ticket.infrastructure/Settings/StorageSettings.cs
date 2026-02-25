using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.infrastructure.Settings
{
    public class StorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SecretSas { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }
}
