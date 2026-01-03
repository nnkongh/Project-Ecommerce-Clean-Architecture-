namespace Ecommerce.Web.ViewModels
{
    public class CartViewModel
    {
        public string UserName { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal GrandTotal
        {
            get
            {
                decimal total = 0;
                foreach(var item in Items)
                {
                    total += item.TotalPrice;
                }
                return total;
            }
        }
    }
}