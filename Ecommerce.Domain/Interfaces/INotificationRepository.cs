using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface INotificationRepository : IRepositoryBase<Notification, int>
    {
        Task<IReadOnlyList<Notification>> GetByUserIdAsync(string userId);
        Task<int> CountUnreadByUserIdAsync(string userId);
        Task MarkAsReadAsync(int id, string userId);
        Task MarkAllAsReadAsync(string userId);
    }
}
