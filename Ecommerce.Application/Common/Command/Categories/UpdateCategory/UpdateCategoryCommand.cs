using Ecommerce.Application.DTOs.Models;
using Ecommerce.Application.DTOs.ModelsRequest.Category;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Categories.UpdateCategory
{
    public sealed record UpdateCategoryCommand(int id, UpdateCategoryRequest update, params Expression<Func<Task, object>>[] propertiesToUpdate) : IRequest<Result<CategoryModel>>
    {
    }
}
