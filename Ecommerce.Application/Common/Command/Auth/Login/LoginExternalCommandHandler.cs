using AutoMapper;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Auth.Login
{
    public sealed class LoginExternalCommandHandler : IRequestHandler<LoginExternalCommand, Result<TokenModel>>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public LoginExternalCommandHandler(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<Result<TokenModel>> Handle(LoginExternalCommand request, CancellationToken cancellationToken)
        {
            if (request.model == null)
            {
                return Result.Failure<TokenModel>(new Error("", "Error"));
            }
            var result = await _authService.LoginExternalProvider(request.model,cancellationToken);
            if (result.IsFailure)
            {
                return Result.Failure<TokenModel>(new Error("", "Error"));
            }
            
            var token = await _tokenService.CreateToken(result.Value,true);
            return Result.Success(token); 
        }
    }
}
