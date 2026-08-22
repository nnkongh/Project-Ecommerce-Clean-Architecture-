using Ecommerce.Domain.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce.Test.TestDomain
{
    public class OrderTest
    {
        [Fact]
        public void AddItem_ShouldAddToList_WhenCartIsEmpty()
        {
            //Arrange
            var address = new Address();
            var order = Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "123";

            //Act
            order.AddItem(image, name, productId, unitprice, quantity);

            //Assert
            Assert.Single(order.Items);
            var items = order.Items.First();

            Assert.Equal(productId,items.ProductId);
            Assert.Equal(quantity,items.Quantity);
            Assert.Equal(unitprice,items.Price);
            Assert.Equal(name,items.ProductName);

        }
        [Fact]
        public void GetTotal_ShouldReturnCorrectValue()
        {
            //Arrange
            var address = new Address();
            var order = Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "abc";

            //Act
            order.AddItem(image, name, productId, unitprice, quantity);
            var result = order.TotalAmount;

            //Assert
            Assert.Equal(60, result);
        }
        [Fact]
        public void AddItem_WhenProductIsExists_ShouldIncreaseQuantity()
        {
            var address = new Address();
            var order = Order.CreateOrder("1","hau", "0123456789", "hau@test.com", address);
            int productId = 1;
            int quantityFirst = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "abc";

            //Act
            order.AddItem(image, name, productId, unitprice, quantityFirst);
            order.AddItem(image, name, productId, unitprice, quantityFirst);

            //Assert
            Assert.Single(order.Items);
            var item = order.Items.First();

            Assert.Equal(quantityFirst + 1, item.Quantity); // tăng thêm 1 mỗi lần gọi
        }

        [Fact]
        public void RemoveItem_ShouldRemoveCompletely()
        {
            var address = new Address();
            var order = Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
            int productId = 1;
            int quantityFirst = 2;
            decimal unitprice = 30;
            string name = "Product1";

            //Act
            order.AddItem("asd",name, productId, unitprice, quantityFirst);
            var item = order.Items.First();
            order.RemoveItem(item);

            //Assert

            Assert.Empty(order.Items);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveOnlyThatItem_WhenMultipleItemExist()
        {
            var address = new Address();
            var order = Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
            order.AddItem("", "product1", 1, 2, 30);
            order.AddItem("", "product2", 2, 3, 30);

            //Act
            var item = order.FindOrderItem(2);
            order.RemoveItem(item!);

            //Assert
            Assert.DoesNotContain(order.Items, x => x.ProductId == 2);
            Assert.Contains(order.Items, x => x.ProductId == 1);
        }

        [Fact]
        public void TotalPrice_ShouldReturnTotalMinusRemovedItem_WhenRemoveOneItem()
        {
            var address = new Address();
            var order = Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
            order.AddItem("", "product1", 1, 30, 2);
            order.AddItem("", "product2", 2, 50, 2);
            var beforeTotal = order.TotalAmount;


            //Act
            var item = order.FindOrderItem(2);
            order.RemoveItem(item!);
            var afterTotal = order.TotalAmount;

            //Assert
            Assert.Equal(160,beforeTotal);
            Assert.Equal(60,afterTotal);

        }
    }
}
