using Ecommerce.Domain.Enum;

namespace Ecommerce.Application.DTOs.Models
{
    public record SubOrderModel : BaseModel
    {
        public int OrderId { get; set; }
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public decimal TotalAmount { get; set; }
        public SubOrderStatus SubOrderStatus { get; set; }
        public IReadOnlyList<OrderItemModel> Items { get; set; } = [];
    }
}
