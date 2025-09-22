using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Models
{
    public class Address : IEquatable<Address>
    {
        public string? District { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Ward { get; set; }
        public bool Equals(Address? other)
        {
            if(other == null)
            {
                return false;
            }
            return District == other.District && City == other.City && PostalCode == other.PostalCode && Ward == other.Ward;
        }
    }
}
