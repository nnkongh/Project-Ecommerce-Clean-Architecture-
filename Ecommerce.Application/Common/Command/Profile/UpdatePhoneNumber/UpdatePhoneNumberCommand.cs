using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile.UpdatePhoneNumber
{
    public sealed record UpdatePhoneNumberCommand(string RequestedBy, string PhoneNumber) : IRequest<Result>
    {
    }
    public sealed class UpdatePhoneHandler : IRequestHandler<UpdatePhoneNumberCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePhoneHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber)) return Result.Failure(new Error("400", "Phone không được để trống"));

            var user = await _userRepository.GetByIdAsync(request.RequestedBy);
            if (user == null) return Result.Failure(new Error("404", "Không tìm thấy người dùng"));

            user.UpdatePhoneNumber(request.PhoneNumber);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
