using Ecommerce.Domain.Enum;

namespace Ecommerce.Web.ViewModels
{
    public class NotificationDetailViewModel
    {
        public NotificationViewModel Notification { get; set; } = null!;
        public OrderViewModel? Order { get; set; }
        public SubOrderViewModel? ShopSubOrder { get; set; }
        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
    }
}
