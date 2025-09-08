using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
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
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public LoginCommandHandler(IAuthenticationService authService, ITokenService tokenService, IMapper mapper)
        {
            _authService = authService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<TokenModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.login.Email) || string.IsNullOrEmpty(request.login.Password))
            {
                return Result.Failure<TokenModel>(new Error("", "Email or Password is empty"));
            }
            try
            {
                var result = await _authService.Login(request.login);
                var mapped = _mapper.Map<UserModel>(result);
                var token = await _tokenService.CreateToken(result.Value, true);
                return Result.Success(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Some error occur while processing", ex);
            }
            

        }
    }
}
