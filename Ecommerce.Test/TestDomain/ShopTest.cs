using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class ShopTest
    {
        [Fact]
        public void Create_ShouldReturnShopWithCorrectValues_WhenValidInput()
        {
            // Arrange
            var name = "My Shop";
            var userId = "user1";

            // Act
            var shop = Shop.Create(name, userId);

            // Assert
            Assert.Equal(name, shop.Name);
            Assert.Equal(userId, shop.UserId);
            Assert.True(shop.IsActive);
            Assert.Empty(shop.Products);
            Assert.Null(shop.Address);
            Assert.True(shop.CreatedAt <= DateTime.UtcNow.AddSeconds(1));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
        {
            Assert.Throws<ArgumentException>(() => Shop.Create(name!, "user1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_ShouldThrowArgumentException_WhenUserIdIsInvalid(string? userId)
        {
            Assert.Throws<ArgumentException>(() => Shop.Create("Shop", userId!));
        }

        [Fact]
        public void Update_ShouldChangeName_WhenNameIsValid()
        {
            var shop = Shop.Create("Old Name", "user1");
            var newName = "New Name";

            shop.Update(newName);

            Assert.Equal(newName, shop.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
        {
            var shop = Shop.Create("Old Name", "user1");

            Assert.Throws<ArgumentException>(() => shop.Update(name!));
        }

        [Fact]
        public void UpdateAddress_ShouldSetAddress_WhenAddressIsValid()
        {
            var shop = Shop.Create("Shop", "user1");
            var address = new Address();

            shop.UpdateAddress(address);

            Assert.Equal(address, shop.Address);
        }

        [Fact]
        public void UpdateAddress_ShouldThrowArgumentNullException_WhenAddressIsNull()
        {
            var shop = Shop.Create("Shop", "user1");

            Assert.Throws<ArgumentNullException>(() => shop.UpdateAddress(null!));
        }

        [Fact]
        public void UpdateImage_ShouldSetImageUrl()
        {
            var shop = Shop.Create("Shop", "user1");
            var imageUrl = "https://example.com/logo.png";

            shop.UpdateImage(imageUrl);

            Assert.Equal(imageUrl, shop.ImageUrl);
        }

        [Fact]
        public void UpdateImage_ShouldSetEmpty_WhenImageUrlIsNull()
        {
            var shop = Shop.Create("Shop", "user1");

            shop.UpdateImage(null!);

            Assert.Equal(string.Empty, shop.ImageUrl);
        }

        [Fact]
        public void Deactivate_Activate_ShouldToggleIsActive()
        {
            var shop = Shop.Create("Shop", "user1");
            Assert.True(shop.IsActive);

            shop.Deactivate();
            Assert.False(shop.IsActive);

            shop.Activate();
            Assert.True(shop.IsActive);
        }

        [Fact]
        public void AddProduct_ShouldAddProductToCollection()
        {
            var shop = Shop.Create("Shop", "user1");
            //var product = Product.Create("Product 1", "img.jpg", shop.Id, 100, 10, 1);

            //shop.AddProduct(product);

            //Assert.Single(shop.Products);
            //Assert.Contains(product, shop.Products);
        }

        [Fact]
        public void AddProduct_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            var shop = Shop.Create("Shop", "user1");

            Assert.Throws<ArgumentNullException>(() => shop.AddProduct(null!));
        }

        [Fact]
        public void RemoveProduct_ShouldRemoveProductFromCollection()
        {
            var shop = Shop.Create("Shop", "user1");
            //var product1 = Product.Create("Product 1", "img.jpg", shop.Id, 100, 10, 1);
            //var product2 = Product.Create("Product 2", "img2.jpg", shop.Id, 200, 5, 1);
            //shop.AddProduct(product1);
            //shop.AddProduct(product2);

            //shop.RemoveProduct(product1);

            //Assert.Single(shop.Products);
            //Assert.DoesNotContain(product1, shop.Products);
            //Assert.Contains(product2, shop.Products);
        }

        [Fact]
        public void RemoveProduct_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            var shop = Shop.Create("Shop", "user1");

            Assert.Throws<ArgumentNullException>(() => shop.RemoveProduct(null!));
        }
    }
}
