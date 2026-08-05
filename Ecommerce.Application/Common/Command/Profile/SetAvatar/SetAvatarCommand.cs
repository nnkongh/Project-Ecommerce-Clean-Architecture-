using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile.SetAvatar
{
    public sealed record SetAvatarCommand(byte[] FileBytes, string FileName, string RequestedBy) : IRequest<Result>
    {
    }
    public sealed class SetAvatarHandler : IRequestHandler<SetAvatarCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public SetAvatarHandler(IUserRepository userRepository, IPhotoService photoService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetAvatarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.FileBytes == null) return Result.Failure(new Error("400", "Avatar không được để trống"));

                var user = await _userRepository.GetByIdAsync(request.RequestedBy);
                if (user == null) return Result.Failure(new Error("404", "Không tìm thấy người dùng"));

                if (user.ImageUrl != null)
                {
                    await _photoService.DeletePhotoAsync(user.ImageUrl);
                }
                var atavarUrl = await _photoService.CreatePhotoAsync(request.FileBytes, request.FileName);
                user.UpdateAvatarUrl(atavarUrl);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("400", $"{ex.Message}"));
            }
        }
    }
}
