using System.Text.Json.Serialization;

namespace Ecommerce.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public List<ItemWishList> Items { get; set; } = new List<ItemWishList>();

        public void AddItem(int ProductId, string productName)
        {
            var existing = Items.FirstOrDefault(x => x.ProductId == ProductId);
            if (existing == null)
            {
                Items.Add(new ItemWishList()
                {
                    ProductId = ProductId,
                    ProductName = productName,
                });
            }
        }
        public void RemoveItem(int ProductId)
        {
            var existing = Items.FirstOrDefault(x => x.ProductId == ProductId);
            if (existing != null)
            {
                Items.Remove(existing);
            }
        }
        public void ClearItem()
        {
            Items.Clear();
        }
    }
}