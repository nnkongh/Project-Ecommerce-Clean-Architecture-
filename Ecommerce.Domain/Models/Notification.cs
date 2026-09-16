using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public int? OrderId { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public User User { get; private set; }
        public Notification() { }

        public static Notification Create(string title, string description, string userId, int? orderId = null)
        {
            var notification = new Notification
            {
                Description = description,
                Title = title,
                UserId = userId,
                OrderId = orderId,
                IsRead = false,
                CreatedDate = DateTime.Now
            };
            return notification;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
