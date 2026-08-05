using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.CreateAddress
{
    public sealed record CreateAddressCommand(CreateAddressRequest Request, string UserId) : IRequest<Result<UserAddressModel>>;
}
