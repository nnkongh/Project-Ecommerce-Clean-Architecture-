using Ecommerce.Domain.Models;

namespace Ecommerce.Test.TestDomain
{
    public class OrderTest
    {
        private static readonly int ShopId = 1;
        private static readonly string ShopName = "Shop1";

        private static Order CreateDefaultOrder()
        {
            var address = Address.Create("District", "City", "Province", "Street", "Ward");
            return Order.CreateOrder("1", "hau", "0123456789", "hau@test.com", address);
        }

        private static SubOrder CreateDefaultSubOrder(int orderId)
        {
            return SubOrder.Create(orderId, ShopId, ShopName);
        }

        [Fact]
        public void AddItem_ShouldAddToList_WhenCartIsEmpty()
        {
            //Arrange
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "123";

            //Act
            subOrder.AddItem(image, name, productId, unitprice, quantity);
            order.AddSubOrder(subOrder);

            //Assert
            Assert.Single(order.SubOrders);
            var items = order.SubOrders.First().Items.First();

            Assert.Equal(productId, items.ProductId);
            Assert.Equal(quantity, items.Quantity);
            Assert.Equal(unitprice, items.Price);
            Assert.Equal(name, items.ProductName);
        }

        [Fact]
        public void GetTotal_ShouldReturnCorrectValue()
        {
            //Arrange
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "abc";

            //Act
            subOrder.AddItem(image, name, productId, unitprice, quantity);
            order.AddSubOrder(subOrder);
            var result = order.TotalAmount;

            //Assert
            Assert.Equal(60, result);
        }

        [Fact]
        public void AddItem_WhenProductIsExists_ShouldIncreaseQuantity()
        {
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            int productId = 1;
            int quantityFirst = 2;
            decimal unitprice = 30;
            string name = "Product1";
            string image = "abc";

            //Act
            subOrder.AddItem(image, name, productId, unitprice, quantityFirst);
            subOrder.AddItem(image, name, productId, unitprice, quantityFirst);
            order.AddSubOrder(subOrder);

            //Assert
            Assert.Single(order.SubOrders.First().Items);
            var item = order.SubOrders.First().Items.First();

            Assert.Equal(quantityFirst + quantityFirst, item.Quantity);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveCompletely()
        {
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            int productId = 1;
            int quantityFirst = 2;
            decimal unitprice = 30;
            string name = "Product1";

            //Act
            subOrder.AddItem("asd", name, productId, unitprice, quantityFirst);
            var item = subOrder.FindOrderItem(productId);
            subOrder.RemoveItem(item!);
            order.AddSubOrder(subOrder);

            //Assert
            Assert.Empty(order.SubOrders.First().Items);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveOnlyThatItem_WhenMultipleItemExist()
        {
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            subOrder.AddItem("", "product1", 1, 2, 30);
            subOrder.AddItem("", "product2", 2, 3, 30);

            //Act
            var item = subOrder.FindOrderItem(2);
            subOrder.RemoveItem(item!);
            order.AddSubOrder(subOrder);

            //Assert
            Assert.DoesNotContain(order.SubOrders.First().Items, x => x.ProductId == 2);
            Assert.Contains(order.SubOrders.First().Items, x => x.ProductId == 1);
        }

        [Fact]
        public void TotalPrice_ShouldReturnTotalMinusRemovedItem_WhenRemoveOneItem()
        {
            var order = CreateDefaultOrder();
            var subOrder = CreateDefaultSubOrder(order.Id);
            subOrder.AddItem("", "product1", 1, 30, 2);
            subOrder.AddItem("", "product2", 2, 50, 2);
            order.AddSubOrder(subOrder);
            var beforeTotal = subOrder.TotalAmount;

            //Act
            var item = subOrder.FindOrderItem(2);
            subOrder.RemoveItem(item!);
            var afterTotal = subOrder.TotalAmount;

            //Assert
            Assert.Equal(160, beforeTotal);
            Assert.Equal(60, afterTotal);
        }
    }
}
