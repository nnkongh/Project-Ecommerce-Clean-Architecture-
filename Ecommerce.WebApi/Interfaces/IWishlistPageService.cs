using Ecommerce.Domain.Models;
using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Interfaces
{
    public interface IWishlistPageService
    {
        Task<WishlistViewModel> GetWishlist(string userName);
        Task AddItem(string userName, Guid productId);
        Task RemoveItem(Guid wishListId, Guid productId);
    }
}
