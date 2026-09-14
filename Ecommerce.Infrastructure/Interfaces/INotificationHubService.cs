using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface INotificationHubService
    {
        Task SendNotificationToUserAsync(string id, object message);

    }
}
