using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.UpdateAddress
{
    public sealed class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<UserAddressModel>>
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAddressCommandHandler(IUserAddressRepository addressRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _addressRepo = addressRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserAddressModel>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepo.GetByIdAsync(request.Request.Id);
            if (address == null)
            {
                return Result.Failure<UserAddressModel>(new Error("404", "Không tìm thấy địa chỉ"));
            }
            if (address.UserId != request.UserId)
            {
                return Result.Failure<UserAddressModel>(new Error("403", "Bạn không có quyền cập nhật địa chỉ này"));
            }

            address.Update(
                request.Request.Street,
                request.Request.Ward,
                request.Request.District,
                request.Request.City,
                request.Request.Province);

            if (request.Request.IsDefault && !address.IsDefault)
            {
                var existingDefault = await _addressRepo.GetDefaultAddressAsync(request.UserId);
                if (existingDefault != null && existingDefault.Id != address.Id)
                {
                    existingDefault.UnsetAsDefault();
                }
                address.SetAsDefault();
            }

            _addressRepo.Update(address);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var mapped = _mapper.Map<UserAddressModel>(address);
            return Result.Success(mapped);
        }
    }
}
