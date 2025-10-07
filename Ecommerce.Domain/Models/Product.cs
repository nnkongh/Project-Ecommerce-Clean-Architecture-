﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Wishlist> Wishlists { get; set; } = [];


        public Product(string name, string description, string imageUrl, decimal price, int stock, int categoryId)
        {
            if(price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
            if(stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative");
            }
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new ArgumentException("Price cannot be negative");
            Price = newPrice;
        }
        public void AdjustStock(int stock)
        {
            if(stock < 0) throw new ArgumentException("Price cannot be negative");
            Stock += stock;
        }
    }
}
