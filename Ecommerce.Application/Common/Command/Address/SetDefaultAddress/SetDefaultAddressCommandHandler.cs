using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.SetDefaultAddress
{
    public sealed class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommand, Result>
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SetDefaultAddressCommandHandler(IUserAddressRepository addressRepo, IUnitOfWork unitOfWork)
        {
            _addressRepo = addressRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepo.GetByIdAsync(request.Id);
            if (address == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy địa chỉ"));
            }
            if (address.UserId != request.UserId)
            {
                return Result.Failure(new Error("403", "Bạn không có quyền thay đổi địa chỉ này"));
            }

            var existingDefault = await _addressRepo.GetDefaultAddressAsync(request.UserId);
            if (existingDefault != null && existingDefault.Id != address.Id)
            {
                existingDefault.UnsetAsDefault();
            }

            address.SetAsDefault();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
