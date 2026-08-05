using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Address.DeleteAddress
{
    public sealed class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result>
    {
        private readonly IUserAddressRepository _addressRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAddressCommandHandler(IUserAddressRepository addressRepo, IUnitOfWork unitOfWork)
        {
            _addressRepo = addressRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepo.GetByIdAsync(request.Id);
            if (address == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy địa chỉ"));
            }
            if (address.UserId != request.UserId)
            {
                return Result.Failure(new Error("403", "Bạn không có quyền xóa địa chỉ này"));
            }

            await _addressRepo.Delete(address);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
