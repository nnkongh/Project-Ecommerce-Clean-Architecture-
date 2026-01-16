using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.ModelsRequest.Product
{
    public record UpdateProductRequest(string Name, string Description, string ImageUrl, decimal? Price, int? Stock);
}
