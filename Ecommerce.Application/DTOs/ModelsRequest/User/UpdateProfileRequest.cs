using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.User
{
    public sealed record UpdateProfileRequest 
    {
        public string? UserName { get; set; }
        public AddressRequest? Address { get; set; }

    }
    public class AddressRequest
    {
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
    }
}
