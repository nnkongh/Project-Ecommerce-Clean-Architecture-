using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Address;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.UpdateAddress
{
    public sealed record UpdateAddressCommand(UpdateAddressRequest Request, string UserId) : IRequest<Result<UserAddressModel>>;
}
