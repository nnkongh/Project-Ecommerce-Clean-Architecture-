using Ecommerce.Domain.Models;
using Ecommerce.Web.ViewModels;
using Newtonsoft.Json;

namespace Ecommerce.Web.Models
{
    public class CheckoutCartRequest
    {
        public AddressViewModel? Address { get; set; }
        public string? UserName { get; set; }
        public string? Phonenumber { get; set; }
    }
}
