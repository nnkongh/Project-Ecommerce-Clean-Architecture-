using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Wishlists.RemoveItemInWishlist
{
    public sealed class RemoveItemWishlistCommandHandler : IRequestHandler<RemoveItemWishlistCommand, Result>
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IUnitOfWork _uow;
        public RemoveItemWishlistCommandHandler(IWishlistRepository wishlistRepo, IUnitOfWork uow)
        {
            _wishlistRepo = wishlistRepo;
            _uow = uow;
        }

        public async Task<Result> Handle(RemoveItemWishlistCommand request, CancellationToken cancellationToken)
        {
            var wishlist = await _wishlistRepo.GetWishlistWithItemByIdAsync(request.wishlistId);
            if (wishlist == null)
            {
                return Result.Failure(new Error("", "Wishlist không tồn tại"));
            }
            var item = wishlist.Items.FirstOrDefault(x => x.ProductId == request.productId);
            if (item == null)
            {
                return Result.Failure(new Error("", "Sản phẩm không tồn tại trong wishlist"));
            }
            wishlist.RemoveItem(request.productId);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
