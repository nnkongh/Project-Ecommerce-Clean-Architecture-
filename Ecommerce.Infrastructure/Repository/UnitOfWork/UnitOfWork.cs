using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IProductRepository _productRepo;
        private ICartRepository _cartRepo;
        private IOrderRepository _orderRepo;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public IProductRepository ProductRepository => _productRepo ??= new ProductRepository(_dbContext);

        public ICartRepository CartRepository => _cartRepo ??= new CartRepository(_dbContext);

        public IOrderRepository OrderRepository => _orderRepo ??= new OrderRepository(_dbContext);


        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
