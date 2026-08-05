using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.CreateAddress
{
    public sealed class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<UserAddressModel>>
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(IUserAddressRepository addressRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserAddressModel>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            if (request.Request.IsDefault)
            {
                var existingDefault = await _addressRepo.GetDefaultAddressAsync(request.UserId);
                if (existingDefault != null)
                {
                    existingDefault.UnsetAsDefault();
                }
            }

            var address = UserAddress.Create(
                request.UserId,
                request.Request.Street,
                request.Request.Ward,
                request.Request.District,
                request.Request.City,
                request.Request.Province,
                request.Request.IsDefault);

            await _addressRepo.AddAsync(address);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var mapped = _mapper.Map<UserAddressModel>(address);
            return Result.Success(mapped);
        }
    }
}
