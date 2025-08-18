using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Enum
{
    public enum OrderStatus
    {
        Pending,       // Order has been placed but not yet processed
        Processing,    // Order is being prepared
        Finished,      // Order has been completed
    }
}
