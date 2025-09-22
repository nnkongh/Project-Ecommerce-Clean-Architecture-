using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.User
{
    public sealed record UpdateUserRequest 
    {
        public string ImageUrl { get; set; } = string.Empty;
        public Address Address { get; set; }
    }
}
