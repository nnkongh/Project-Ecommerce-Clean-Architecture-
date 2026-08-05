using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Interfaces.UnitOfWork;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile.EmailConfirmed
{
    public sealed record EmailConfirmCommand(string RequestedBy, string Token) : IRequest<Result>
    {
    }
    public sealed class EmailConfirmedHandler : IRequestHandler<EmailConfirmCommand, Result>
    {
        private readonly IUserAuthTokenService _userAuthTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;

        public EmailConfirmedHandler(
            IUserAuthTokenService userAuthTokenService,
            IUserRepository userRepository,
            IUnitOfWork uow)
        {
            _userAuthTokenService = userAuthTokenService;
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<Result> Handle(EmailConfirmCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.RequestedBy);
            if (user == null)
            {
                return Result.Failure(new Error("404", "Không tìm thấy người dùng"));
            }

            var confirmResult = await _userAuthTokenService.ConfirmEmailAsync(request.RequestedBy, request.Token);
            if (confirmResult.IsFailure)
            {
                return confirmResult;
            }

            user.MarkAsEmailConfirmed();
            await _userRepository.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
