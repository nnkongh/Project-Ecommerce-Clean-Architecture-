using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Auth.Forgot
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
    {
        private readonly IAuthenticationService _authService;

        public ForgotPasswordCommandHandler(IAuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.model.Email))
            {
                return Result.Failure<string>(new Error("", "Email is empty"));
            }
            var result = await _authService.ForgotPassword(request.model);
            if (result.IsFailure)
            {
                return result;
            }
            return result.Value;
        }
    }
}
