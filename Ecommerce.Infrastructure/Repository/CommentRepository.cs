using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class CommentRepository : GenericRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetAllComentsByProductIdAsync(int productId)
        {
            return await _context.Comments.Where(c => c.ProductId == productId)
                                 .Include(c => c.User)
                                 .ToListAsync();
        }

        public async Task<Comment?> GetByIdWithUserAsync(int id)
        {
            return await _context.Comments.Include(c => c.User)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
