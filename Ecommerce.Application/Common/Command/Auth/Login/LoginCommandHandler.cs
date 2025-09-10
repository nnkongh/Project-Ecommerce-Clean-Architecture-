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
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenModel>>
    {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public LoginCommandHandler(IAuthenticationService authService, IMapper mapper, ITokenService tokenService)
        {
            _authService = authService;
            _mapper = mapper;
            _tokenService = tokenService;
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
