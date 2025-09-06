using System.Text.Json.Serialization;

namespace Ecommerce.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public List<ProductWishList> Items { get; set; } = new List<ProductWishList>();

        public void AddItem(int ProductId)
        {
            var existing = Items.FirstOrDefault(x => x.ProductId == ProductId);
            if (existing == null)
            {
                Items.Add(new ProductWishList()
                {
                    ProductId = ProductId,
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
    }
}