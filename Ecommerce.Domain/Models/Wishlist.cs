using Ecommerce.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Ecommerce.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public User? User { get; set; }
        public List<ItemWishList> Items { get; set; } = new List<ItemWishList>();


        public static Wishlist Create(string userId, string name)
        {
            var wishlist = new Wishlist()
            {
                UserId = userId,
                Name = name
            };
            return wishlist;
        }
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
            else
            {
                throw new DuplicateProductException(ProductId);
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