using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class CartExpirationService : ICartExpirationService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IUnitOfWork _uow;
        public CartExpirationService(ICartRepository cartRepo, IUnitOfWork uow)
        {
            _cartRepo = cartRepo;
            _uow = uow;
        }

        public async Task HandleExpiredCartAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var expiredCarts = await _cartRepo.GetExpiredCartsAsync(now);

            if (expiredCarts.Count == 0) return;

            foreach (var cart in expiredCarts)
            {
                cart.MarkAsExpired();
            }
            _cartRepo.DeleteRange(expiredCarts);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
