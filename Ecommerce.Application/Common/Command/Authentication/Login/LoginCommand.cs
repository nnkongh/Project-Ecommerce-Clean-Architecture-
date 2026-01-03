using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Authentication.Login
{
    public sealed record LoginCommand(LoginModel login) : IRequest<Result<TokenModel>>
    {
    }
}
