using Ecommerce.Domain.Models;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class ProductTest
    {
        [Fact]
        public void AddProduct_ShouldThrowException_WhenQuantiyLessThanZero()
        {
            Product product;
            Assert.Throws<ArgumentException>(() => product = new Product("ABC", "Test", "ASDFASDFASDF", -30, 3, 2));

        }
        [Fact]
        public void AddProduct_ShouldThrowException_WhenPriceLessThanZero()
        {
            Product product;
            Assert.Throws<ArgumentException>(() => product = new Product("ABC", "Test", "ASDFASDFASDF", 1, -30, 2));

        }
        [Fact]
        public void AddProduct_ShouldReturnTrueProp_WhenCreateNewProduct()
        {
            var name = "ABC";
            var des = "des";
            var imageUrl = "abc.jpg";
            var stock = 30;
            var price = 30;
            var categoryId = 1;
            var product = new Product(name, des, imageUrl, price, stock, categoryId);

            Assert.Equal(name, product.Name);
            Assert.Equal(des, product.Description);
            Assert.Equal(price, product.Price);
            Assert.Equal(imageUrl, product.ImageUrl);
            Assert.Equal(stock, product.Stock);
            Assert.Equal(categoryId, product.CategoryId);
        }
        [Fact]
        public void AddProduct_ShouldNotCrash_WhenPriceIsLarge()
        {
            var max = int.MaxValue;
            var exception = Record.Exception(() => new Product("Abc", "des", "bvc", max, 1, 1));

            Assert.Null(exception);
        }
        [Fact]
        public void AddProduct_ShouldNotCrash_WhenStockIsLarge()
        {
            var max = int.MaxValue;
            var exception = Record.Exception(() => new Product("Abc", "des", "bvc", 1, max, 1));

            Assert.Null(exception);
        }
        [Fact]
        public void UpdatePrice_ShouldBeUpdate_WhenUpdate()
        {
            var name = "ABC";
            var des = "des";
            var imageUrl = "abc.jpg";
            var stock = 30;
            var price = 30;
            var categoryId = 1;
            var product = new Product(name, des, imageUrl, price, stock, categoryId);


            var updatePrice = 40;
            product.UpdatePrice(updatePrice);

            Assert.Equal(updatePrice,product.Price);
            Assert.NotEqual(price, product.Price);
        }
        [Fact]
        public void UpdatePrice_ShouldThrowException_WhenPriceIsNegative()
        {
            var name = "ABC";
            var des = "des";
            var imageUrl = "abc.jpg";
            var stock = 30;
            var price = -30;
            var categoryId = 1;

            Assert.Throws<ArgumentException>(() => new Product(name, des, imageUrl, price, stock, categoryId));
        }
        [Fact]
        public void UpdateStock_ShouldThrowException_WhenStockIsNegative()
        {
            var name = "ABC";
            var des = "des";
            var imageUrl = "abc.jpg";
            var stock = -30;
            var price = 30;
            var categoryId = 1;

            Assert.Throws<ArgumentException>(() => new Product(name, des, imageUrl, price, stock, categoryId));
        }
    }
}
