using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Address : IEquatable<Address>
    {
        public string? District { get; private set; }
        public string? City { get; private set; }
        public string? Province { get; private set; }
        public string? Ward { get; private set; }
        public string? Street { get; private set; }

        public bool Equals(Address? other)
        {
            if(other == null)
            {
                return false;
            }
            return District == other.District && City == other.City && Street == other.Street && Ward == other.Ward && Province == other.Province;
        }
        public static Address Create(string? district, string? city, string? province, string? street, string? ward)
        {
            var address = new Address
            {
                District = district,
                City = city,
                Street = street,
                Province = province,
                Ward = ward
            };
            return address;
        }
    }
}
