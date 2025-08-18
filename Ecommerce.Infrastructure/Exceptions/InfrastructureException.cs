using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        internal InfrastructureException(string message) : base(message)
        {
        }
        internal InfrastructureException(string message, Exception exception) : base(message, exception)
        {
        }

    }
    
}
