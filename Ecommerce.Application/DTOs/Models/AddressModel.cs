using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Models
{
    public sealed record AddressModel
    {
        public string? District { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Ward { get; set; }
    }
}
