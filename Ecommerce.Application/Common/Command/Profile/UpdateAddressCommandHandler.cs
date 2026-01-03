using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.User;
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

namespace Ecommerce.Application.Common.Command.Profile
{
    public sealed class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<AddressModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UpdateAddressCommandHandler(IUnitOfWork uow, IUserRepository userRepo, IMapper mapper)
        {
            _uow = uow;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Result<AddressModel>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepo.GetByIdAsync(request.userId);
            if(user == null)
            {
                return Result.Failure<AddressModel>(new Error("", "User is not found"));
            }
            if(user.Address == null)
            {
                user.Address = Address.Create(
                    request.update.PostalCode,
                    request.update.District,
                    request.update.Ward!,
                    request.update.City);
            }
            else
            {
                user.Address = Address.Update(request.update.PostalCode,
                    request.update.District,
                    request.update.Ward!,
                    request.update.City);
            }
           
            await _userRepo.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<AddressModel>(user);
            return Result.Success(mapped);
        }
    }
}
