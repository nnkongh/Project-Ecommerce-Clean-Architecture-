using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.DeleteAddress
{
    public sealed record DeleteAddressCommand(int Id, string UserId) : IRequest<Result>;
}
