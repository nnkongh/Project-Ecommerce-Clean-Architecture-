using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Auth.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenModel>>
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        public LoginCommandHandler(IAuthenticationService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<Result<TokenModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.login.Email) || string.IsNullOrEmpty(request.login.Password))
            {
                return Result.Failure<TokenModel>(new Error("", "Email or Password is empty"));
            }
            var result = await _authService.Login(request.login); //
            if (result.IsFailure)
            {
                return Result.Failure<TokenModel>(result.Error);
            }
            var token = await _tokenService.CreateToken(result.Value, true);
            return Result.Success(token);
        }


    }
}
