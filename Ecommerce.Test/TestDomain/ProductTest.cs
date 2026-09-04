using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class ProductTest
    {
        private static Product CreateProduct(
            string name = "ABC",
            string imageUrl = "abc.jpg",
            int shopId = 1,
            decimal price = 30,
            int stock = 30,
            int categoryId = 1,
            string? description = "des")
        {
            return Product.Create(name, imageUrl, shopId, price, stock, categoryId, 1, description);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenNameIsEmpty()
        {
            Assert.Throws<DomainException>(() => CreateProduct(name: ""));
        }
        [Fact]
        public void Create_ShouldThrowException_WhenPriceLessThanZero()
        {
            Assert.Throws<DomainException>(() => CreateProduct(price: -30));
        }
        [Fact]
        public void Create_ShouldThrowException_WhenStockLessThanZero()
        {
            Assert.Throws<DomainException>(() => CreateProduct(stock: -30));
        }
        [Fact]
        public void Create_ShouldThrowException_WhenImageUrlIsEmpty()
        {
            Assert.Throws<DomainException>(() => CreateProduct(imageUrl: " "));
        }
        [Fact]
        public void Create_ShouldReturnTrueProp_WhenCreateNewProduct()
        {
            var name = "ABC";
            var des = "des";
            var imageUrl = "abc.jpg";
            var stock = 30;
            var price = 30;
            var categoryId = 1;
            var shopId = 2;
            var product = CreateProduct(name, imageUrl, shopId, price, stock, categoryId, des);

            Assert.Equal(name, product.Name);
            Assert.Equal(des, product.Description);
            Assert.Equal(price, product.Price);
            Assert.Equal(imageUrl, product.ImageUrl);
            Assert.Equal(stock, product.Stock);
            Assert.Equal(categoryId, product.ParentCategoryId);
            Assert.Equal(shopId, product.ShopId);
            Assert.True(product.IsActive);
        }
        [Fact]
        public void Create_ShouldNotCrash_WhenPriceIsLarge()
        {
            var max = int.MaxValue;
            var exception = Record.Exception(() => CreateProduct(price: max));

            Assert.Null(exception);
        }
        [Fact]
        public void Create_ShouldNotCrash_WhenStockIsLarge()
        {
            var max = int.MaxValue;
            var exception = Record.Exception(() => CreateProduct(stock: max));

            Assert.Null(exception);
        }
        [Fact]
        public void UpdateProduct_ShouldUpdatePrice_WhenPriceProvided()
        {
            var product = CreateProduct();
            var updatePrice = 40;

            product.UpdateProduct(price: updatePrice);

            Assert.Equal(updatePrice, product.Price);
            Assert.NotEqual(30, product.Price);
        }
        [Fact]
        public void UpdateProduct_ShouldThrowException_WhenPriceIsNegative()
        {
            var product = CreateProduct();

            Assert.Throws<DomainException>(() => product.UpdateProduct(price: -1));
        }
        [Fact]
        public void UpdateProduct_ShouldThrowException_WhenStockIsNegative()
        {
            var product = CreateProduct();

            Assert.Throws<DomainException>(() => product.UpdateProduct(stock: -1));
        }
        [Fact]
        public void UpdateProduct_ShouldKeepOldValue_WhenParameterIsNull()
        {
            var product = CreateProduct();

            product.UpdateProduct();

            Assert.Equal("ABC", product.Name);
            Assert.Equal(30, product.Price);
            Assert.Equal(30, product.Stock);
        }
        [Fact]
        public void AdjustStock_ShouldIncreaseOrDecreaseStock()
        {
            var product = CreateProduct(stock: 10);

            product.AdjustStock(5);
            Assert.Equal(15, product.Stock);

            product.AdjustStock(-20);
            Assert.Equal(-5, product.Stock);
        }
    }
}
