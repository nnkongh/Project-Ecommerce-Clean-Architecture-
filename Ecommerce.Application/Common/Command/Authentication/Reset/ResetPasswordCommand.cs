using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Authentication.Reset
{
    public sealed record ResetPasswordCommand(ResetPasswordModel model) : IRequest<Result>
    {
    }
}
