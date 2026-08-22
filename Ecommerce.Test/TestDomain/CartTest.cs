using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class CartTest
    {
        [Fact]
        public void CountItemInCart_WhenAddItem_ShouldReturnActualItemInList()
        {
            var cart = Cart.CreateCart("user1");
            cart.AddItem(1, "Item1", 2, 3);
            cart.AddItem(2, "Item2", 1, 3);
            cart.AddItem(3, "Item3", 1, 3);
            var count = cart.Items.Count;


            Assert.NotEmpty(cart.Items);
            Assert.Equal(3, count);
        }
        [Fact]
        public void AddItem_WhenItemInListIsEmpty_ShouldReturnListItem()
        {
            var user = "abc";
            var productId = 1;
            var quantity = 2;
            var unitprice = 30;
            var name = "product1";

            var cart = Cart.CreateCart(user);
            cart.AddItem(productId, name, quantity, unitprice);

            Assert.Single(cart.Items);
            var item = cart.Items.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(unitprice, item.UnitPrice);
            Assert.Equal(name, item.ProductName);
            Assert.Equal(quantity * unitprice, item.TotalPrice);
        }
        [Fact]
        public void AddItem_WhenItemIsExists_ShouldIncreaseQuantity()
        {
            var cart = Cart.CreateCart("Abc");
            int productId = 1;
            int firstQuantity = 2;
            decimal unitPrice = 30;
            string productName = "product1";

            // Act
            cart.AddItem(productId, productName, firstQuantity, unitPrice);
            cart.AddItem(productId, productName, firstQuantity, unitPrice);

            // Assert
            Assert.Single(cart.Items); // vẫn chỉ có 1 item
            var item = cart.Items.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(firstQuantity + 1, item.Quantity); // IncreaseQuantity tăng thêm 1
            Assert.Equal(unitPrice, item.UnitPrice);
            Assert.Equal(productName, item.ProductName);

        }
        [Fact]
        public void UpdateQuantity_WhenItemIsExists_ShouldSetNewQuantity()
        {
            var cart = Cart.CreateCart("Abc");
            int productId = 1;
            int quantity = 3;
            int newQuantity = 1;
            decimal unitPrice = 30;
            string prodcutName = "product1";

            cart.AddItem(productId, prodcutName, quantity, unitPrice);
            cart.UpdateQuantity(productId, newQuantity);

            var item = cart.Items.First();
            Assert.Equal(newQuantity, item.Quantity);

        }
        [Fact]
        public void RemoveItem_WhenItemIsExists_ShoudlRemoveAllItem()
        {
            var cart = Cart.CreateCart("Abc");
            int productId = 1;
            int quantity = 3;
            decimal unitPrice = 30;
            string prodcutName = "product1";

            cart.AddItem(productId, prodcutName, quantity, unitPrice);
            var item = cart.GetItem(productId);
            cart.RemoveItem(item!);


            Assert.True(cart.Items.Count == 0);
            Assert.NotNull(cart);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public void ClearItem_WhenItemIsExists_ShoudlClearAllItems()
        {
            var cart = Cart.CreateCart("Abc");

            cart.AddItem(1, "XYZ", 2, 30);
            cart.AddItem(2, "PBC", 3, 30);
            cart.AddItem(3, "LAS", 1, 30);


            cart.Clear();

            Assert.Empty(cart.Items);
            Assert.NotNull(cart);
        }
        [Fact]
        public void AddItem_WhenQuantityLessThanZero_ShoudlThrowException()
        {
            var cart = Cart.CreateCart("Abc");

            Assert.Throws<DomainException>(() => cart.AddItem(1, "ABC", -3, 30));
        }
        [Fact]
        public void AddItem_WhenUnitPriceLessThanZero_ShoudlThrowException()
        {
            var cart = Cart.CreateCart("Abc");

            Assert.Throws<DomainException>(() => cart.AddItem(1, "ABC", 3, -30));
        }
        [Fact]
        public void AddItem_WhenProductNameIsEmpty_ShoudlThrowException()
        {
            var cart = Cart.CreateCart("Abc");

            Assert.Throws<DomainException>(() => cart.AddItem(1, "", 3, 30));
        }
        [Fact]
        public void AddItem_WhenQuantityIsVeryLarge_ShouldHandleWithoutOverFlow()
        {
            var cart = Cart.CreateCart("abc");
            var max = int.MaxValue;
            cart.AddItem(1, "product1", max, 30);

            var item = cart.Items.First();
            Assert.Equal(max, item.Quantity);
            Assert.Equal(max * 30m, item.TotalPrice);
        }
    }
}
