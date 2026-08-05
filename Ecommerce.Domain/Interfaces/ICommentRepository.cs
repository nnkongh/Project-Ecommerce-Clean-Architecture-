using Ecommerce.Domain.Interfaces.Base;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICommentRepository : IRepositoryBase<Comment,int>
    {
        Task<IEnumerable<Comment>> GetAllComentsByProductIdAsync(int productId);
        Task<Comment?> GetByIdWithUserAsync(int id);
    }
}
