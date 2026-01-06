using AutoMapper;
using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Ecommerce.Application.Common.Command.AuthenticationExternal
{
    public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, ExternalLoginResult>
    {
        private readonly IExternalLoginService _externalLoginService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ExternalLoginCommandHandler> _logger;
        public ExternalLoginCommandHandler(IUserRepository userRepository,
            IMapper mapper,
            IExternalLoginService externalLoginService,
            IUnitOfWork uow,
            ILogger<ExternalLoginCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _externalLoginService = externalLoginService;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ExternalLoginResult> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var info = request.info;

            var user = await _externalLoginService.FindByExternalLoginAsync(info.Provider, info.ProviderKey);

            if (user != null)
            {
                return new ExternalLoginResult
                {
                    IsSuccess = true,
                    User = user,
                };
            }

            
            var existingUser = await _userRepository.FindByEmailAsync(info.Email);
            if (existingUser != null)
            {
                var externalIdentity = new ExternalIdentity
                {
                    Provider = info.Provider,
                    ProviderKey = info.ProviderKey,
                };
                await _externalLoginService.AddExternalLoginToExistingUserAsync(existingUser.Id,externalIdentity);
                var userModel = _mapper.Map<UserModel>(existingUser);
                return new ExternalLoginResult
                {
                    IsSuccess = true,
                    User = userModel, 
                };
            }
            var externalUserInfo = new ExternalUserInfo
            {
                Email = info.Email,
                Name = info.Name,
            };
            var newExternalIdentiy = new ExternalIdentity
            {
                Provider = info.Provider,
                ProviderKey = info.ProviderKey,
            };
            var createdUser = await _externalLoginService.CreateUserFromExternalAsync(externalUserInfo, newExternalIdentiy, cancellationToken);

            return new ExternalLoginResult
            {
                IsSuccess = true,
                User = createdUser,
                
            };
        }
    }
}