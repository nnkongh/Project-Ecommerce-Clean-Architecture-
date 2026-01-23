using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Users;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Profile
{
    public sealed record UpdateProfileCommand(string userId, ProfileModel update) : IRequest<Result<ProfileModel>>
    {
    }
}
