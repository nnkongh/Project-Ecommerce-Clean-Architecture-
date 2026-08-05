using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Common.Command.Profile
{
    public sealed record UpdateProfileCommand(string RequestedBy, ProfileModel Request) : IRequest<Result>
    {
    }

    public sealed class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;

        public UpdateProfileHandler(IUserRepository userRepository, IUnitOfWork uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.RequestedBy);
            if (user == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy người dùng"));
            }

            if (!string.IsNullOrWhiteSpace(request.Request.PhoneNumber))
            {
                user.UpdatePhoneNumber(request.Request.PhoneNumber);
            }

            if (!string.IsNullOrWhiteSpace(request.Request.ImageUrl))
            {
                user.UpdateAvatarUrl(request.Request.ImageUrl);
            }

            if (request.Request.Address != null)
            {
                var address = Ecommerce.Domain.Models.Address.Create(
                    request.Request.Address.District,
                    request.Request.Address.City,
                    request.Request.Address.Province,
                    request.Request.Address.Street,
                    request.Request.Address.Ward);
                user.UpdateAddress(address);
            }

            _uow.UserRepository.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}