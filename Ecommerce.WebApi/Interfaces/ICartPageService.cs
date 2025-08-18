using Ecommerce.Web.ViewModels;

namespace Ecommerce.Web.Interfaces
{
    public interface ICartPageService
    {
        Task<CartViewModel> GetCart(string userName);
        Task AddItem(string userName, Guid productId);
        Task RemoveItem(Guid cartId, Guid cartItemId);
    }
}
