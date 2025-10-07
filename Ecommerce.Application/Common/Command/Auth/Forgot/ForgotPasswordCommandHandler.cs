using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Auth.Forgot
{
    public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IAuthService _authService;

        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.model.Email))
            {
                return Result.Failure<string>(new Error("", "Email is empty"));
            }
            var result = await _authService.ForgotPassword(request.model);
            if (result.IsFailure)
            {
                return Result.Failure<string>(result.Error);
            }
            return Result.Success();
        }
    }
}
