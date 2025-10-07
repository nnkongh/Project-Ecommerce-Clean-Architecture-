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
            cart.AddItem(1, 2, 3, "Item1");
            cart.AddItem(2, 1, 3, "Item2");
            cart.AddItem(3, 1, 3, "Item3");
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
            cart.AddItem(productId, quantity, unitprice, name);

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
            int firstQuantity = 1;
            int secondQuantity = 2;
            decimal unitPrice = 30;
            string productName = "product1";

            // Act
            cart.AddItem(productId, firstQuantity, unitPrice, productName);
            cart.AddItem(productId, secondQuantity, unitPrice, productName);

            // Assert
            Assert.Single(cart.Items); // vẫn chỉ có 1 item
            var item = cart.Items.First();
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(firstQuantity + secondQuantity, item.Quantity); // Quantity = 3
            Assert.Equal(unitPrice, item.UnitPrice);
            Assert.Equal(productName, item.ProductName);

        }
        [Fact]
        public void RemoveItemQuantity_WhenItemIsExists_ShouldDecreaseQuantity()
        {
            var cart = Cart.CreateCart("Abc");
            int productId = 1;
            int quantity = 3;
            int removedquantiy = 2;
            decimal unitPrice = 30;
            string prodcutName = "product1";

            cart.AddItem(productId,quantity, unitPrice, prodcutName);
            cart.ReduceItemQuantity(productId, removedquantiy);

            var item = cart.Items.First();
            Assert.Equal(1, item.Quantity);

        }
        [Fact]
        public void RemoveItem_WhenItemIsExists_ShoudlRemoveAllItem()
        {
            var cart = Cart.CreateCart("Abc");
            int productId = 1;
            int quantity = 3;
            decimal unitPrice = 30;
            string prodcutName = "product1";

            cart.AddItem(productId, quantity, unitPrice, prodcutName);
            cart.RemoveItem(productId);


            Assert.True(cart.Items.Count == 0);
            Assert.NotNull(cart);
            Assert.Empty(cart.Items);
        }
        [Fact]
        public void ClearItem_WhenItemIsExists_ShoudlClearAllItems()
        {
            var cart = Cart.CreateCart("Abc");

            cart.AddItem(1, 2, 30, "XYZ");
            cart.AddItem(2,3,30,"PBC");
            cart.AddItem(3,1,30,"LAS");


            cart.Clear();

            Assert.Empty(cart.Items);
            Assert.NotNull(cart);
        }
        [Fact]
        public void AddItem_WhenQuantityLessThanZero_ShoudlThrowException()
        {
            var cart = Cart.CreateCart("Abc");

            Assert.Throws<ArgumentException>(() => cart.AddItem(1,-3,30,"ABC"));
        }
        [Fact]
        public void AddItem_WhenStockLessThanZero_ShoudlThrowException()
        {
            var cart = Cart.CreateCart("Abc");

            Assert.Throws<ArgumentException>(() => cart.AddItem(1, 3, -30, "ABC"));
        }
        [Fact]
        public void AddItem_WhenQuantityIsVeryLarge_ShouldHandleWithoutOverFlow()
        {
            var cart = Cart.CreateCart("abc");
            var max = int.MaxValue;
            cart.AddItem(1, max, 30, "product1");

            var item = cart.Items.First();
            Assert.Equal(max, item.Quantity);
            Assert.Equal(max * 30m, item.TotalPrice);
        }
    }
}
