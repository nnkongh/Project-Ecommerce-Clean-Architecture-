using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Exceptions
{
    public class ProviderLinkedException : Exception
    {
        public ProviderLinkedException()
        {
        }
    }
    public class DuplicateProductException : Exception
    {
        public DuplicateProductException(int productId) : base($"Product with ID {productId} already exists in wishlist") { }
    }
}
