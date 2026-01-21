using Ecommerce.Web.Features.Carts;
using Ecommerce.Web.Interface;
using Ecommerce.Web.Services.Strategy;

namespace Ecommerce.Web.Client.Strategy
{
    //----Concrete Creator class-----
    public class CartStrategyFactory : ICartStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CartStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ICartStrategy CreateCartStrategy(IHttpContextAccessor httpContext)
        {
            var isAuthenticated = httpContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            if (isAuthenticated)
            {
                return _serviceProvider.GetRequiredService<CartApiClient>();
            }
            else
            {
                return _serviceProvider.GetRequiredService<CartSessionClient>();
            }

        }
    }
}
