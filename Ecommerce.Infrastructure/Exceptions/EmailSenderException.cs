using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Exceptions
{
    public class EmailSenderException : InfrastructureException
    {
        public EmailSenderException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
