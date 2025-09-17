using AutoMapper;
using Ecommerce.Application.Common.Command.Users.CreateUser;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Users.RegisterUser
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public RegisterCommandHandler(IUnitOfWork uow, IMapper mapper, IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _uow = uow;
            _mapper = mapper;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.register.UserName)
                   || string.IsNullOrWhiteSpace(request.register.Email)
                   || string.IsNullOrWhiteSpace(request.register.Password)
                   || string.IsNullOrWhiteSpace(request.register.ConfirmPassword))
            {
                return Result.Failure(new Error("", "Can not pass empty"));
            }
            // Validation
            if (!string.Equals(request.register.Password, request.register.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure(new Error("", "Password and confirm password do not match"));
            }
            var result = await _authenticationService.Register(request.register);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            var mapped = _mapper.Map<User>(result.Value);
            await _userRepository.AddAsync(mapped);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
