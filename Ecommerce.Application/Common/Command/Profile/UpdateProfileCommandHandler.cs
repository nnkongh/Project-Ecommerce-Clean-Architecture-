using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile
{
    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<ProfileModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UpdateProfileCommandHandler(IUnitOfWork uow, IUserRepository userRepo, IMapper mapper)
        {
            _uow = uow;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Result<ProfileModel>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {


            var user = await _userRepo.GetByIdAsync(request.userId);
            if (user == null)
            {
                return Result.Failure<ProfileModel>(new Error("", "User is not found"));
            }
            if (request.update.UserName != null)
            {
                user.UserName = request.update.UserName;
            }
            if (request.update.ImageUrl != null)
            {
                user.ImageUrl = request.update.ImageUrl;
            }
            if (request.update.PhoneNumber != null)
            {
                user.PhoneNumber = request.update.PhoneNumber;
            }
            if (request.update.Address != null)
            {
                user.Address ??= Address.Create(
                    district: "",
                    city: "",
                    province: "",
                    street: "",
                    ward: "");

                user.Address.Update(
                    district: request.update.Address.District ?? "",
                    city: request.update.Address.City ?? "",
                    province: request.update.Address.Province ?? "",
                    street: request.update.Address.Street ?? "",
                    ward: request.update.Address.Ward ?? "");
            }

            //await _userRepo.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
            var profile = _mapper.Map<ProfileModel>(user);
            return Result.Success(profile);
        }
    }
}
