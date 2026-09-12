using Ecommerce.Domain.Enum;

namespace Ecommerce.Web.ViewModels
{
    public class SubOrderViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public decimal TotalAmount { get; set; }
        public SubOrderStatus SubOrderStatus { get; set; } = SubOrderStatus.Pending;
        public IReadOnlyList<OrderItemViewModel> Items { get; set; } = [];
    }
}