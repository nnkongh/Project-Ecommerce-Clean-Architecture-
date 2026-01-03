using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Authentication.Reset
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IAuthService _authService;

        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.model.Email)
                || string.IsNullOrEmpty(request.model.NewPassword)
                || string.IsNullOrEmpty(request.model.ConfirmPassword))
            {
                return Result.Failure(new Error("", "Some information is empty"));
            }
            if (string.IsNullOrEmpty(request.model.Token))
            {
                return Result.Failure(new Error("", "Token is empty"));
            }
            if (!string.Equals(request.model.ConfirmPassword, request.model.NewPassword, StringComparison.Ordinal))
            {
                return Result.Failure(new Error("", "Password and confirm password does not match"));
            }
            var result = await _authService.ResetPassword(request.model);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            return Result.Success();
        }
    }
}
