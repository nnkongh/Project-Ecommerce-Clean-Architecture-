using AutoMapper;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Authentication.Login
{
    //public sealed class LoginExternalCommandHandler : IRequestHandler<LoginExternalCommand, LoginResponse>
    //{
    //    private readonly IExternalAuthService _externalAuthService;
    //    private readonly IUserRepository _userRepository;
    //    private readonly ITokenService _tokenService;
    //    private readonly IMapper _mapper;


    //    public LoginExternalCommandHandler(ITokenService tokenService, IExternalAuthService externalAuthService, IMapper mapper, IUserRepository userRepository)
    //    {
    //        _tokenService = tokenService;
    //        _externalAuthService = externalAuthService;
    //        _mapper = mapper;
    //        _userRepository = userRepository;
    //    }

    //    public async Task<LoginResponse> Handle(LoginExternalCommand request, CancellationToken cancellationToken)
    //    {
    //        var authResult = await _externalAuthService.ExchangeCodeForTokenAsync(request.model.Code, request.model.RedirectUri);
    //        var user = await _userRepository.FindByEmailAsync(authResult.Email);
    //        var userModel = _mapper.Map<UserModel>(user);
    //        if (user == null)
    //        {
    //            user = new User
    //            {
    //                UserName = authResult.Email,
    //                Email = authResult.Email,
    //            };
    //            await _userRepository.AddAsync(user);

    //        }
    //        var token = await _tokenService.CreateToken(userModel, true);

    //        return new LoginResponse
    //        {
    //            Token = new TokenModel
    //            {
    //                AccessToken = token.AccessToken,
    //                RefreshToken = token.RefreshToken,
    //            },
    //            User = new UserModel
    //            {
    //                Id = user.Id,
    //                UserName = user.UserName,
    //                Email = user.Email,
    //            }
    //        };
    //    }
    //}
}
    