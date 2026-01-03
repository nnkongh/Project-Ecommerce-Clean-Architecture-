namespace Ecommerce.Web.ViewModels
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public Guid ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

    }
}