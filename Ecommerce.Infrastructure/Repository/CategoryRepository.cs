using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Specification;
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
    public class CategoryRepository : GenericRepository<Category,int>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetChildCategoryWithProductAsync(int categoryId)
        {
            return await _context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.ParentId == categoryId);
        }
    }
}
