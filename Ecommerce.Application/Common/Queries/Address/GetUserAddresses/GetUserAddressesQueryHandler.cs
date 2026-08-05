using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Queries.Address.GetUserAddresses
{
    public sealed class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, Result<IReadOnlyList<UserAddressModel>>>
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IMapper _mapper;

        public GetUserAddressesQueryHandler(IUserAddressRepository addressRepo, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<UserAddressModel>>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _addressRepo.GetAddressesByUserIdAsync(request.UserId);
            var mapped = _mapper.Map<IReadOnlyList<UserAddressModel>>(addresses ?? new List<Ecommerce.Domain.Models.UserAddress>());
            return Result.Success(mapped);
        }
    }
}
