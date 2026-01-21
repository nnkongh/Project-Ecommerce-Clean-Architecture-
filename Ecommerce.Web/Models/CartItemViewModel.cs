namespace Ecommerce.Web.ViewModels
{
    public class CartItemViewModel
    {
        //public ProductViewModel Product { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

    }
}