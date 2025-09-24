using AutoMapper;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Queries.Profile.GetProfile
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileModel>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public GetProfileQueryHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Result<ProfileModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.userId);
            if(user == null)
            {
                return Result.Failure<ProfileModel>(new Error("", "User not found"));
            }
            var mapped = _mapper.Map<ProfileModel>(user);
            return Result.Success(mapped);
        }
    }
}
