using AutoMapper;
using Ecommerce.Application.DTOs;
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

namespace Ecommerce.Application.Common.Command.Users
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUnitOfWork uow, IUserRepository userRepo, IMapper mapper)
        {
            _uow = uow;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Result<UserModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepo.GetByIdAsync(request.id);
            if(user == null)
            {
                return Result.Failure<UserModel>(new Error("", "User is not found"));
            }
            var entity = _mapper.Map<User>(request.update);
            var result = await _userRepo.UpdateAsync(user.Id,entity,
                                                    x => x.ImageUrl,
                                                    x => x.Address);
            await _uow.SaveChangesAsync(cancellationToken);
            var mapped = _mapper.Map<UserModel>(user);
            return Result.Success(mapped);
        }
    }
}
