using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, object message);
    }
}
