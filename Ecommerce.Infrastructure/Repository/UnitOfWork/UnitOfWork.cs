using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Infrastructure.Data;
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
        private readonly EcommerceDbContext _dbContext;
        private IProductRepository _productRepo;
        private ICartRepository _cartRepo;
        private IOrderRepository _orderRepo;
        public UnitOfWork(EcommerceDbContext dbContext, IProductRepository productRepo, ICartRepository cartRepo, IOrderRepository orderRepo)
        {
            _dbContext = dbContext;
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
        }

        public IProductRepository ProductRepository => _productRepo ??= new ProductRepository(_dbContext);

        public ICartRepository CartRepository => _cartRepo ??= new CartRepository(_dbContext);


        public IOrderRepository OrderRepository => _orderRepo ??= new OrderRepository(_dbContext);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
