using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Enum
{
    public enum OrderStatus
    {
        Pending,       
        Processing,    
        Finished,    
    }
    public enum CartStatus
    {
        Active,
        Expired
    }
}
