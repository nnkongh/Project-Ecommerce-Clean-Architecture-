using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Shop
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string UserId { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public string ImageUrl { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public User User { get; private set; } 
        public Address? Address { get; private set; }
        public IReadOnlyCollection<Product> Products => _products;
        private readonly List<Product> _products = new List<Product>();

        private Shop() { }
        public static Shop Create(string name, string userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Shop name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId is required.", nameof(userId));

            return new Shop
            {
                Name = name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }
        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Shop name is required.", nameof(name));
            Name = name;
        }
        public void UpdateAddress(Address address)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }
        public void UpdateImage(string  imageUrl)
        {
            ImageUrl = imageUrl ?? string.Empty;
        }
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            _products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            _products.Remove(product);
        }
    }
}
