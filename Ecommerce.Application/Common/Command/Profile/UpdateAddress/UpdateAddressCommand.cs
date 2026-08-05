using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile.UpdateAddress
{
    public sealed record UpdateAddressCommand(string RequestedBy, string? Province = null, string? District = null, string? City = null, string? Ward = null, string? Street = null) : IRequest<Result>
    {
    }
    public sealed class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAddressHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.RequestedBy);
            if (user == null) return Result.Failure(new Error("404", "Không tìm thấy người dùng"));

            var address = Ecommerce.Domain.Models.Address.Create(request.District, request.City, request.Province, request.Street, request.Ward);

            user.UpdateAddress(address);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();

            
        }
    }
}
