﻿using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.ModelsRequest.User;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Users
{
    public sealed record UpdateUserCommand(string id, UpdateUserRequest update) : IRequest<Result<UserModel>>
    {
    }
}
