using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Authentication;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenModel>>
    {
        private readonly IUserAuthenticationService _userAuthService;
        private readonly IUserRepository _userRepo;
        public LoginCommandHandler(IUserRepository userRepo, IUserAuthenticationService userAuthService)
        {
            _userRepo = userRepo;
            _userAuthService = userAuthService;
        }

        public async Task<Result<TokenModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.login.Email) && string.IsNullOrEmpty(request.login.Password))
            {
                return Result.Failure<TokenModel>(new Error("", "Email or Password is empty"));
            }
            var user = await _userRepo.GetUserByEmailAsync(request.login.Email);
            if(user == null)
            {
                return Result.Failure<TokenModel>(new Error("", "User not found"));
            }
            var checkPassword = await _userAuthService.CheckPasswordAsync(user.Id, request.login.Password);
            if (!checkPassword)
            {
                return Result.Failure<TokenModel>(new Error("", "Invalid password"));
            }
            var token = _user

        }
    }
}
