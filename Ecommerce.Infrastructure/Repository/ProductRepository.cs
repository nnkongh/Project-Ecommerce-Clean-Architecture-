using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
using Ecommerce.Domain.Specification.Base;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
