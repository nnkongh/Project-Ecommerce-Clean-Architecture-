using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ecommerce.Web.Authorization
{
    public class OrderOwnerHandler : AuthorizationHandler<OrderOwnerRequirement,Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderOwnerRequirement requirement, Order order)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Task.CompletedTask;
            }
            if (order.CustomerId == userId.Value)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask; ;
        }
    }
}
