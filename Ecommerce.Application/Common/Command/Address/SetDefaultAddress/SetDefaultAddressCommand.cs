using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.SetDefaultAddress
{
    public sealed record SetDefaultAddressCommand(int Id, string UserId) : IRequest<Result>;
}
