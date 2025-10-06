using Ecommerce.Domain.Models;

namespace Ecommerce.Test.TestDomain
{
    public class OrderTest
    {
        [Fact]
        public void AddItem_ShouldAddToList_WhenProductIdNull()
        {
            //Arrange
            var order = new Order();
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";

            //Act
            order.AddItem(productId, quantity, unitprice, name);

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
            var order = new Order();
            int productId = 1;
            int quantity = 2;
            decimal unitprice = 30;
            string name = "Product1";

            //Act
            order.AddItem(productId, quantity, unitprice, name);
            var result = order.TotalAmount;

            //Assert
            Assert.Equal(60, result);
        }
        [Fact]
        public void IncreaseQuantity_WhenProductIsNotNull()
        {
            var order = new Order();
            int productId = 1;
            int quantityFirst = 2;
            int quantitySecond = 3;
            decimal unitprice = 30;
            string name = "Product1";



            //Act
            order.AddItem(productId, quantityFirst, unitprice, name);
            order.AddItem(productId,quantitySecond, unitprice, name);


            //Assert
            Assert.Single(order.Items);
            var item = order.Items.First();

            Assert.Equal(quantityFirst + quantitySecond, item.Quantity);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveCompletely()
        {
            var order = new Order();
            int productId = 1;
            int quantityFirst = 2;
            decimal unitprice = 30;
            string name = "Product1";


            //Act
            order.AddItem(productId, quantityFirst, unitprice, name);
            order.RemoveItem(productId);

            //Assert
            
            Assert.Empty(order.Items);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveOnlyThatItem_WhenMultipleItemExist()
        {
            var order = new Order();
            order.AddItem(1, 2, 30, "product1");
            order.AddItem(2, 1, 20, "product2");


            //Act
            order.RemoveItem(2);

            //Assert
            Assert.DoesNotContain(order.Items, x => x.ProductId == 2);
            Assert.Contains(order.Items, x => x.ProductId == 1);
        }

        [Fact]
        public void TotalPrice_ShouldReturnTotalMinusRemovedItem_WhenRemoveOneItem()
        {
            var order = new Order();
            order.AddItem(1, 2, 30, "product1");
            order.AddItem(2, 2, 50, "product2");
            var beforeTotal = order.TotalAmount;


            //Act   
            order.RemoveItem(2);
            var afterTotal = order.TotalAmount;

            //Assert
            Assert.Equal(160,beforeTotal);
            Assert.Equal(60,afterTotal);

        }
    }
}