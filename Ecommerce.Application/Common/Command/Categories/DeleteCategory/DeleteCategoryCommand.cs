using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Categories.DeleteCategory
{
    public sealed record DeleteCategoryCommand(int id) : IRequest<Result>
    {
    }
}
