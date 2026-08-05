using Ecommerce.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Ecommerce.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; private set; }
        public string? UserId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public User? User { get; private set; }
        public ICollection<ItemWishList> Items => _items.AsReadOnly();

        private List<ItemWishList> _items = new List<ItemWishList>();

        public static Wishlist Create(string userId)
        {
            return new Wishlist()
            {
                UserId = userId,
                CreatedDate = DateTime.Now,
            };
        }
        public void AddItem(int productId, string productName, string? imageUrl = null)
        {
            var item = FindItem(productId);
            if (item != null)
            {
                throw new DomainException("Sản phẩm đã tồn tại trong wishlist");
            }
            var newItem = ItemWishList.Create(productId, productName, imageUrl);
            _items.Add(newItem);
        }
        public void RemoveItem(int productId)
        {
            var item = FindItem(productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
        public void ClearItem()
        {
            _items.Clear();
        }
        private ItemWishList? FindItem(int productId)
        {
            return Items.FirstOrDefault(x => x.ProductId == productId);
        }
    }
}