using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.CRUD.Product
{
    public record CreateProductRequest(string Name, string Description, string ImageUrl, decimal Price, int Stock, int CategoryId)
    {
    }
}
