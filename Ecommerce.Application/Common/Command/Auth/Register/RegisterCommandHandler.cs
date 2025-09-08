using AutoMapper;
using Ecommerce.Application.Common.Command.Users.CreateUser;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Authentication;
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

            // Validation
            if (string.IsNullOrWhiteSpace(request.register.UserName) 
                || string.IsNullOrWhiteSpace(request.register.Email) 
                || string.IsNullOrWhiteSpace(request.register.Password) 
                || string.IsNullOrWhiteSpace(request.register.ConfirmPassword))
            {
                throw new ArgumentNullException("UserName, Email, Password or ConfirmPassword cannot be null or empty");
            }
            ;
            if (!string.Equals(request.register.Password, request.register.ConfirmPassword, StringComparison.Ordinal))
            {
                throw new("Password and ConfirmPassword do not match");
            }
            var result = await _authenticationService.Register(request.register);
            var mapped = _mapper.Map<User>(result);
            await _userRepository.AddAsync(mapped);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
