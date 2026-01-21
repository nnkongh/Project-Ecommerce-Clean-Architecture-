using Ecommerce.Web.Services.Strategy;

namespace Ecommerce.Web.Interface
{
    public interface ICartStrategyFactory
    {
        ICartStrategy CreateCartStrategy(IHttpContextAccessor httpContext);
    }
}
