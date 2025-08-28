using Ecommerce.Application.DTOs.CRUD.Category;
using Ecommerce.Application.DTOs.Product;
using Ecommerce.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Command.Categories.CreateCategory
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest create) : IRequest<Result<CategoryModel>>
    {
    }
}
