using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketAzure.application
{
   public class NotFoundError : Error
    {
        public NotFoundError(string message)
            : base(message, ErrorType.NotFound)
        {
        }
    }

    public class InvalidError : Error
    {
        public InvalidError(string message)
            : base(message, ErrorType.Invalid)
        {
        }
    }
}
