using Ecommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;

        public int? ParentId { get; private set; }
        public Category? Parent { get; private set; }

        public ICollection<Category> Children => _children.AsReadOnly();
        public ICollection<Product> Products => _products.AsReadOnly();

        private List<Category> _children = new List<Category>();
        private List<Product> _products = new List<Product> { };

        
        private Category() { }

        public static Category Create(string name, int? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Tên category không được để trống");

            return new Category
            {
                Name = name,
                ParentId = parentId 
            };
        }
        public void UpdateCategory(string name, int? parentId = null)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Tên category không được để trống");
            Name = name;
            ParentId = parentId;

        }
    }
}
