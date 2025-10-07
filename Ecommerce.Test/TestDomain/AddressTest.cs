using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    public class AddressTest
    {
        [Fact]
        public void AddAddress_WhenAddressIsNull_ShouldCreateNewAddress()
        {
            var address = Address.Create("ABC", "City1", "153A", "P3");

            Assert.Equal("ABC",address.District);
            Assert.Equal("City1",address.City);
            Assert.Equal("153A",address.PostalCode);
            Assert.Equal("P3",address.Ward);

        }
        [Fact]
        public void UpdateAddress_WhenAddressIsNotNull_ShouldUpdateNewAddress()
        {
            var address = Address.Create("ABC", "City1", "153A", "P3");

            address = Address.Update("XYZ", "City2", "143A", "P2");

            Assert.Equal("XYZ", address.District);
            Assert.Equal("City2", address.City);
            Assert.Equal("143A", address.PostalCode);
            Assert.Equal("P2", address.Ward);

            Assert.DoesNotContain("ABC", x => address.District == "ABC");
            Assert.DoesNotContain("City1", x => address.City == "City1");
            Assert.DoesNotContain("153A", x => address.PostalCode == "153A");
            Assert.DoesNotContain("P3", x => address.Ward == "P3");

        }
        [Fact]
        public void Equals_ShouldReturnTrue_WhenAllPropertiesAreEqual()
        {
            var addr1 = new Address
            {
                District = "A",
                City = "B",
                PostalCode = "123",
                Ward = "C"
            };

            var addr2 = new Address
            {
                District = "A",
                City = "B",
                PostalCode = "123",
                Ward = "C"
            };

            Assert.True(addr1.Equals(addr2));
        }
        [Fact]
        public void Equals_ShouldReturnFalse_WhenAnyPropertyDiffers()
        {
            var addr1 = new Address { District = "A", City = "B", PostalCode = "123", Ward = "C" };
            var addr2 = new Address { District = "X", City = "B", PostalCode = "123", Ward = "C" };

            Assert.False(addr1.Equals(addr2));
        }
        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherIsNull()
        {
            var addr = new Address { District = "A" };

            Assert.False(addr.Equals(null));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenComparedToItself()
        {
            var addr = new Address { District = "A" };

            Assert.True(addr.Equals(addr));
        }
    }
}
