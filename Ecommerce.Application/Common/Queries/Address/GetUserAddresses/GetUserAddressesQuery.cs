using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Address.GetUserAddresses
{
    public sealed record GetUserAddressesQuery(string UserId) : IRequest<Result<IReadOnlyList<UserAddressModel>>>;
}
